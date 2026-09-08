using FleetPulse.Application.Abstractions.Repositories;
using MediatR;

namespace FleetPulse.Application.Features.Assets.Queries.GetActiveAssets
{
    public class GetActiveAssetsQueryHandler : IRequestHandler<GetActiveAssetsQuery, IReadOnlyList<AssetDto>>
    {
        private readonly IAssetRepository _assetRepository;

        public GetActiveAssetsQueryHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<IReadOnlyList<AssetDto>> Handle(GetActiveAssetsQuery request, CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetActiveAssetsAsync();

            return assets.Select(a => new AssetDto(
                a.Id,
                a.Name,
                a.SerialNumber,
                a.Type,
                a.DriverId,
                a.IsActive
            )).ToList();
        }
    }
}