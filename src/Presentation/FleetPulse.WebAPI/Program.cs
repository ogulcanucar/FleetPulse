using FleetPulse.Application;
using FleetPulse.Application.Abstractions.Messaging;
using FleetPulse.Application.Abstractions.Services;
using FleetPulse.Persistence;
using FleetPulse.WebAPI.Hubs;
using FleetPulse.WebAPI.Messaging;
using FleetPulse.WebAPI.Services;
using FleetPulse.WebAPI.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddHostedService<TelemetryBackgroundService>();
builder.Services.AddHostedService<GeofenceViolationConsumer>();
builder.Services.AddScoped<ITelemetryPublisher, SignalRTelemetryPublisher>();
builder.Services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddSingleton<RabbitMqConnectionManager>();
builder.Services.AddSignalR();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<FleetHub>("/fleetHub");
app.Run();