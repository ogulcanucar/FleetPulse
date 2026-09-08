using FleetPulse.Application.Abstractions.Repositories;
using MediatR;

namespace FleetPulse.Application.Features.Assets.Queries.GetAssetTelemetry
{
    public class GetAssetTelemetryQueryHandler : IRequestHandler<GetAssetTelemetryQuery, List<TelemetryDto>>
    {
        private readonly ITelemetryRepository _telemetryRepository;

        public GetAssetTelemetryQueryHandler(ITelemetryRepository telemetryRepository)
        {
            _telemetryRepository = telemetryRepository;
        }

        public async Task<List<TelemetryDto>> Handle(GetAssetTelemetryQuery request, CancellationToken cancellationToken)
        {
            var telemetries = await _telemetryRepository.GetLatestTelemetryByAssetIdAsync(request.AssetId);

            return telemetries.Select(t => new TelemetryDto(
                t.Id,
                t.AssetId,
                t.Latitude,
                t.Longitude,
                t.Speed,
                t.EngineStatus,
                t.Timestamp
            )).ToList();
        }
    }
}