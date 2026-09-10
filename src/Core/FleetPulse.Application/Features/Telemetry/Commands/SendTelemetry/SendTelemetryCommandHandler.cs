using FleetPulse.Application.Abstractions.Repositories;
using FleetPulse.Domain.Common.Helpers;
using FleetPulse.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetPulse.Application.Abstractions.Services;
using FleetPulse.Application.Abstractions.DTOs.Telemetry;

namespace FleetPulse.Application.Features.Telemetry.Commands.SendTelemetry
{
    public class SendTelemetryCommandHandler(
     ITelemetryRepository telemetryRepository,
     IGeofenceRepository geofenceRepository,
     IGeofenceViolationRepository violationRepository,
     IAssetRepository assetRepository,
     ILogger<SendTelemetryCommandHandler> logger, ITelemetryPublisher telemetryPublisher) : IRequestHandler<SendTelemetryCommand, string>
    {
        private readonly ITelemetryRepository _telemetryRepository = telemetryRepository;
        private readonly IGeofenceRepository _geofenceRepository = geofenceRepository;
        private readonly IGeofenceViolationRepository _violationRepository = violationRepository;
        private readonly IAssetRepository _assetRepository = assetRepository;
        private readonly ILogger<SendTelemetryCommandHandler> _logger = logger;
        private readonly ITelemetryPublisher _telemetryPublisher = telemetryPublisher;

        public async Task<string> Handle(SendTelemetryCommand request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.AssetId);

            var activeGeofences = await _geofenceRepository.GetActiveGeofencesAsync();
            foreach (var geofence in activeGeofences)
            {
                var distance = GeoCalculator.CalculateDistanceInMeters(
                    geofence.CenterLatitude,
                    geofence.CenterLongitude,
                    request.Latitude,
                    request.Longitude
                );

                if (distance > geofence.RadiusInMeters)
                {
                    var violation = new GeofenceViolation
                    {
                        AssetId = request.AssetId,
                        GeofenceId = geofence.Id,
                        DriverId = asset?.DriverId ?? string.Empty,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        DistanceInMeters = distance,
                        ViolationTime = DateTime.UtcNow
                    };

                    await _violationRepository.AddAsync(violation);
                    _logger.LogWarning("🚨 GEOFENCE İHLALİ! '{AssetName}' adlı araç sınır dışına çıktı! Uzaklık: {Distance:F2} metre",
         asset?.Name ?? "Bilinmeyen Araç", distance);
                }
            }

            var telemetry = new TelemetryData
            {
                AssetId = request.AssetId,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Speed = request.Speed,
                EngineStatus = request.EngineStatus,
                Timestamp = DateTime.UtcNow
            };

            await _telemetryRepository.AddAsync(telemetry);
            var telemetryDto = new TelemetryBroadcastDto
            {
                AssetId = telemetry.AssetId,
                Latitude = telemetry.Latitude,
                Longitude = telemetry.Longitude,
                Speed = telemetry.Speed,
                EngineStatus = telemetry.EngineStatus,
                Timestamp = telemetry.Timestamp
            };
            await _telemetryPublisher.PublishTelemetryAsync(telemetryDto);


            return telemetry.Id;
        }
    }
}