using FleetPulse.Application.Abstractions.Repositories;
using FleetPulse.Application.Features.Telemetry.Commands.SendTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR;

namespace FleetPulse.WebAPI.Workers;

public class TelemetryBackgroundService(
    ILogger<TelemetryBackgroundService> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    private readonly ILogger<TelemetryBackgroundService> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);
    private readonly Random _random = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Telemetri Simülasyon Servisi başlatıldı.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var assetRepository = scope.ServiceProvider.GetRequiredService<IAssetRepository>();

                var assets = await assetRepository.ListAllAsync(); ;
                var assetList = assets?.ToList();

                if (assetList == null || assetList.Count == 0)
                {
                    _logger.LogWarning("Simülasyon için veritabanında hiç araç bulunamadı! Lütfen önce Swagger üzerinden araç ekleyin.");
                    await Task.Delay(_interval, stoppingToken);
                    continue;
                }

                foreach (var asset in assetList)
                {
                    var latitude = GetRandomLatitude();
                    var longitude = GetRandomLongitude();
                    var speed = GetRandomSpeed();

                    var command = new SendTelemetryCommand(asset.Id, latitude, longitude, speed, speed > 0);

                    await mediator.Send(command, stoppingToken);

                    _logger.LogInformation("Simülasyon gönderildi -> Asset: {AssetName} ({AssetId}), Lat: {Lat}, Lng: {Lng}", asset.Name, asset.Id, latitude, longitude);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Telemetri simülasyonu çalışırken hata oluştu.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private double GetRandomLatitude() => 41.0082 + (_random.NextDouble() - 0.5) * 0.01;
    private double GetRandomLongitude() => 28.9784 + (_random.NextDouble() - 0.5) * 0.01;
    private double GetRandomSpeed() => _random.Next(20, 120);
}