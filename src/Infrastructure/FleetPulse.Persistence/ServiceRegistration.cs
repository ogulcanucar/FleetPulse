using FleetPulse.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using FleetPulse.Persistence.Repositories;
using FleetPulse.Application.Abstractions.Repositories;

namespace FleetPulse.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection service)
        {
            service.AddSingleton<MongoDbContext>();
            service.AddScoped(typeof (IRepository<>), typeof(GenericRepository<>));
            service.AddScoped<IAssetRepository, AssetRepository>();
            service.AddScoped<IDriverRepository, DriverRepository>();
            service.AddScoped<ITelemetryRepository, TelemetryRepository>();
            service.AddScoped<IGeofenceRepository, GeofenceRepository>();
            service.AddScoped<IGeofenceViolationRepository, GeofenceViolationRepository>();

        }
    }
}
