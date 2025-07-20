using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TransferService.Application.Behaviors;
using TransferService.Application.Commands.CreateTransfer;
using TransferService.Application.DomainEventHandlers;
using TransferService.Application.Interfaces;
using TransferService.Application.Repositories;
using TransferService.Infrastructure.Configurations;
using TransferService.Infrastructure.Messaging;
using TransferService.Persistence;
using TransferService.Persistence.Contexts;
using TransferService.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<TransferCreatedDomainEventHandler>());

builder.Services.AddValidatorsFromAssembly(typeof(CreateTransferCommandValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<TransferDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITransferRepository, TransferRepository>();

builder.Services.AddScoped<IEventPublisher, EventPublisher>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddEntityFrameworkOutbox<TransferDbContext>(o =>
    {
        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

        cfg.Host(options.Host, "/", h =>
        {
            h.Username(options.Username);
            h.Password(options.Password);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransferDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
