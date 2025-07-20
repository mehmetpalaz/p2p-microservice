using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NotificationService.API.Consumers;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Handlers;
using NotificationService.Infrastructure.Configurations;
using NotificationService.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);


var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<NotificationDbContext>();
builder.Services.AddScoped<ITransferCreatedEventHandler, TransferCreatedEventHandler>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<TransferCreatedEventConsumer>();
    
    x.AddEntityFrameworkOutbox<NotificationDbContext>(o =>
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

        cfg.ReceiveEndpoint("notification_queue", e =>
        {
            e.ConfigureConsumer<TransferCreatedEventConsumer>(context);
        });
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
