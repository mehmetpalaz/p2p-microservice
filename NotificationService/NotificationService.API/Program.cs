using NotificationService.API.Messaging;
using NotificationService.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Handlers;
using NotificationService.Application.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<NotificationDbContext>();
builder.Services.AddScoped<ITransferCreatedEventHandler, TransferCreatedEventHandler>();

builder.Services.AddHostedService<TransferCreatedConsumer>();

var app = builder.Build();

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
