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

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<Ride> filter = Builders<Ride>.Filter.Eq(m => m.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    public async Task<Ride?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<Ride> filter = Builders<Ride>.Filter.Eq(m => m.Id, id);

        Ride? ride = await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);

        return ride;
    }
}
