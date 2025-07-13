using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransferService.Application.Behaviors;
using TransferService.Application.Commands.CreateTransfer;
using TransferService.Application.Interfaces;
using TransferService.Application.Repositories;
using TransferService.Infrastructure.Background;
using TransferService.Infrastructure.Messaging;
using TransferService.Persistence;
using TransferService.Persistence.Contexts;
using TransferService.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateTransferCommand).Assembly);
});

builder.Services.AddValidatorsFromAssembly(typeof(CreateTransferCommandValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<TransferDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITransferRepository, TransferRepository>();

builder.Services.AddSingleton<IRabbitMqEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddHostedService<OutboxPublisherService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
