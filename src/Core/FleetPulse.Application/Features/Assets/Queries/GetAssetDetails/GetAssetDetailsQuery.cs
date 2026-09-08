using MediatR;

namespace FleetPulse.Application.Features.Assets.Queries.GetAssetDetails
{
    public record GetAssetDetailsQuery(string AssetId) : IRequest<AssetDetailDto>;
}