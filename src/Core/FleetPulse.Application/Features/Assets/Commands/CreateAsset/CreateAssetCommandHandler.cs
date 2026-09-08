using FleetPulse.Application.Abstractions.Repositories;
using FleetPulse.Domain.Entities;
using MediatR;

namespace FleetPulse.Application.Features.Assets.Commands.CreateAsset
{
    public class CreateAssetCommandHandler : IRequestHandler<CreateAssetCommand, string>
    {
        private readonly IAssetRepository _assetRepository;

        public CreateAssetCommandHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<string> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
        {
            var existingAsset = await _assetRepository.GetBySerialNumberAsync(request.SerialNumber);
            if (existingAsset != null)
            {
                throw new InvalidOperationException($"'{request.SerialNumber}' seri numaralı varlık zaten kayıtlı.");
            }

            var asset = new Asset
            {
                Name = request.Name,
                SerialNumber = request.SerialNumber,
                Type = request.Type,
                CreatedAt = DateTime.UtcNow
            };

            await _assetRepository.AddAsync(asset);
            return asset.Id;
        }
    }
}