using FleetPulse.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FleetPulse.Persistence.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDbSettings:ConnectionString"] ?? "mongodb://localhost:27017";
        var databaseName = configuration["MongoDbSettings:DatabaseName"] ?? "FleetPulseDb";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Driver> Drivers => _database.GetCollection<Driver>("Drivers");
    public IMongoCollection<TelemetryData> TelemetryData => _database.GetCollection<TelemetryData>("TelemetryData");
    public IMongoCollection<Geofence> Geofences => _database.GetCollection<Geofence>("Geofences");
    public IMongoCollection<GeofenceViolation> GeofenceViolations => _database.GetCollection<GeofenceViolation>("GeofenceViolations");
    public IMongoCollection<Asset> Assets => _database.GetCollection<Asset>("Assets");
}