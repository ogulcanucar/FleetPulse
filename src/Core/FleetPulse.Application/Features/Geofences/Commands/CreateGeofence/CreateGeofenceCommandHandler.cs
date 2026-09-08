using FleetPulse.Application.Abstractions.Repositories;
using FleetPulse.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetPulse.Application.Features.Geofences.Commands.CreateGeofence
{
    public class CreateGeofenceCommandHandler : IRequestHandler<CreateGeofenceCommand, string>
    {
        private readonly IGeofenceRepository _geofenceRepository;

        public CreateGeofenceCommandHandler(IGeofenceRepository geofenceRepository)
        {
            _geofenceRepository = geofenceRepository;
        }

        public async Task<string> Handle(CreateGeofenceCommand request, CancellationToken cancellationToken)
        {
            var geofence = new Geofence
            {
                Name = request.Name,
                CenterLatitude = request.CenterLatitude,
                CenterLongitude = request.CenterLongitude,
                RadiusInMeters = request.RadiusInMeters,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _geofenceRepository.AddAsync(geofence);
            return geofence.Id;
        }
    }
}
