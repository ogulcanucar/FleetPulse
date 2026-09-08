using FleetPulse.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Geofences.Queries.GetAssetViolations
{
    public class GetAssetViolationsQueryHandler : IRequestHandler<GetAssetViolationsQuery, List<GeofenceViolationDto>>
    {
        private readonly IGeofenceViolationRepository _violationRepository;

        public GetAssetViolationsQueryHandler(IGeofenceViolationRepository violationRepository)
        {
            _violationRepository = violationRepository;
        }

        public async Task<List<GeofenceViolationDto>> Handle(GetAssetViolationsQuery request, CancellationToken cancellationToken)
        {
            var violations = await _violationRepository.GetViolationsByAssetIdAsync(request.VehicleId);

            return violations.Select(v => new GeofenceViolationDto(
                v.Id,
                v.AssetId,
                v.GeofenceId,
                v.DriverId,
                v.Latitude,
                v.Longitude,
                v.DistanceInMeters,
                v.ViolationTime
            )).ToList();
        }
    }
}
