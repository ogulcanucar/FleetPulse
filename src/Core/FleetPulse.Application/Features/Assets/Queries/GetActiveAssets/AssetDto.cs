using FleetPulse.Domain.Enums;

namespace FleetPulse.Application.Features.Assets.Queries.GetActiveAssets
{
    public record AssetDto(
        string Id,
        string Name,
        string SerialNumber,
        AssetType Type,
        string? DriverId,
        bool IsActive
    );
}