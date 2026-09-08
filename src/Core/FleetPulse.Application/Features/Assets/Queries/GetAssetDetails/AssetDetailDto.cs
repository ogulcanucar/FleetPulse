using FleetPulse.Domain.Enums;

namespace FleetPulse.Application.Features.Assets.Queries.GetAssetDetails
{
    public record AssetDetailDto(
        string Id,
        string Name,
        string SerialNumber,
        AssetType Type,
        bool IsActive,
        Dictionary<string, object> Attributes,
        DriverInfoDto? Driver
    );

    public record DriverInfoDto(
        string Id,
        string FullName,
        string PhoneNumber,
        string LicenseType
    );
}