using FleetPulse.Application.Abstractions.Repositories;
using MediatR;

namespace FleetPulse.Application.Features.Telemetry.Queries.GetSpeedingAlarms;

public class GetSpeedingAlarmsQueryHandler : IRequestHandler<GetSpeedingAlarmsQuery, List<SpeedingAlarmDto>>
{
    private readonly ITelemetryRepository _telemetryRepository;

    public GetSpeedingAlarmsQueryHandler(ITelemetryRepository telemetryRepository)
    {
        _telemetryRepository = telemetryRepository;
    }

    public async Task<List<SpeedingAlarmDto>> Handle(GetSpeedingAlarmsQuery request, CancellationToken cancellationToken)
    {
        var speedingData = await _telemetryRepository.GetSpeedingAlarmsAsync();

        return speedingData.Select(t => new SpeedingAlarmDto(
            t.Id,
            t.AssetId,
            t.Speed,
            t.Latitude,
            t.Longitude,
            t.Timestamp
        )).ToList();
    }
}