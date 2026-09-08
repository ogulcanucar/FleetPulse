using FleetPulse.Application.Abstractions.Repositories;
using MediatR;

namespace FleetPulse.Application.Features.Assets.Queries.GetAssetDetails
{
    public class GetAssetDetailsQueryHandler : IRequestHandler<GetAssetDetailsQuery, AssetDetailDto>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IDriverRepository _driverRepository;

        public GetAssetDetailsQueryHandler(
            IAssetRepository assetRepository,
            IDriverRepository driverRepository)
        {
            _assetRepository = assetRepository;
            _driverRepository = driverRepository;
        }

        public async Task<AssetDetailDto> Handle(GetAssetDetailsQuery request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.AssetId);
            if (asset == null)
                throw new KeyNotFoundException("Varlık (Asset) bulunamadı.");

            DriverInfoDto? driverInfo = null;

            if (!string.IsNullOrEmpty(asset.DriverId))
            {
                var driver = await _driverRepository.GetByIdAsync(asset.DriverId);
                if (driver != null)
                {
                    driverInfo = new DriverInfoDto(
                        driver.Id,
                        $"{driver.FirstName} {driver.LastName}",
                        driver.PhoneNumber,
                        driver.LicenseType
                    );
                }
            }

            return new AssetDetailDto(
                asset.Id,
                asset.Name,
                asset.SerialNumber,
                asset.Type,
                asset.IsActive,
                asset.Attributes,
                driverInfo
            );
        }
    }
}