using FleetPulse.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Driver.Commands.AssignDriver
{
    public class AssignDriverToAssetCommandHandler : IRequestHandler<AssignDriverToAssetCommand, bool>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IDriverRepository _driverRepository;

        public AssignDriverToAssetCommandHandler(IAssetRepository assetRepository, IDriverRepository driverRepository)
        {
            _assetRepository = assetRepository;
            _driverRepository = driverRepository;
        }

        public async Task<bool> Handle(AssignDriverToAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.AssetId);
            if (asset == null) throw new KeyNotFoundException("Varlık (Asset) bulunamadı.");

            var driver = await _driverRepository.GetByIdAsync(request.DriverId);
            if (driver == null) throw new KeyNotFoundException("Sürücü bulunamadı.");

            asset.DriverId = request.DriverId;
            await _assetRepository.UpdateAsync(asset);

            return true;
        }
    }
}