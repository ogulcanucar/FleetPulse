namespace FleetPulse.Application.Features.Assets.Queries.GetAssetTelemetry
{
    public record TelemetryDto(
        string Id,
        string AssetId,
        double Latitude,
        double Longitude,
        double Speed,
        bool EngineStatus,
        DateTime Timestamp
    );
}