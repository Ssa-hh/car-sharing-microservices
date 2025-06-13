using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ssa.CarSharing.Rides.Domain.Rides;

namespace Ssa.CarSharing.Rides.Infrastructure.Database.Repositories;

internal class RideRepository : IRideRepository
{
    private readonly IMongoCollection<Ride> _collection;

    public RideRepository(IMongoClient mongoClient, IOptions<MongoDbOptions> options)
    {

        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<Ride>(options.Value.RideCollectionName);
    }

    public async Task AddAsync(Ride ride, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(ride);
    }

    public async Task<long> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<Ride> filter = Builders<Ride>.Filter.Eq(r => r.Id, id);
        var result = await _collection.DeleteOneAsync(filter);

        return  result.DeletedCount;
    }

    public async Task<Ride?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<Ride> filter = Builders<Ride>.Filter.Eq(r => r.Id, id);

        Ride? ride = await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);

        return ride;
    }

    public async Task<long> ReplaceAsync(Ride ride, CancellationToken cancellationToken = default)
    {
        FilterDefinition<Ride> filter = Builders<Ride>.Filter.Eq(r => r.Id, ride.Id);

        var options = new ReplaceOptions { IsUpsert = true };

        var result = await _collection.ReplaceOneAsync(filter, ride, options, cancellationToken);

        return result.ModifiedCount;
    }
}
