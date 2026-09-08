using MediatR;


namespace FleetPulse.Application.Features.Telemetry.Commands.SendTelemetry
{
    public record SendTelemetryCommand(
    string AssetId,
    double Latitude,
    double Longitude,
    double Speed,
    bool EngineStatus
) : IRequest<string>;
}
