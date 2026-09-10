using FleetPulse.Application;
using FleetPulse.Persistence;
using FleetPulse.WebAPI.Hubs;
using FleetPulse.WebAPI.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddHostedService<TelemetryBackgroundService>();
builder.Services.AddSignalR();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<FleetHub>("/fleetHub");
app.Run();