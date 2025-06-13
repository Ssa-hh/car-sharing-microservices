using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Ssa.CarSharing.Rides.Domain.Rides;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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

    public async Task<IEnumerable<Ride>> FindAsync(DateOnly? startDate, string? pickupCity, string? dropOffCity, bool onlyOpenOrComplete = true, CancellationToken cancellationToken = default)
    {
        var builder = Builders<Ride>.Filter;

        List<FilterDefinition<Ride>> criterias = new List<FilterDefinition<Ride>>();

        if (startDate.HasValue)
        {
            var startDateTime = startDate.Value.ToDateTime(new TimeOnly());
            criterias.Add(builder.Gte<DateTime>(r => r.StartsAtUtc, startDateTime));
        }

        if (!string.IsNullOrWhiteSpace(pickupCity))
        {
            var queryExpr = new BsonRegularExpression(new Regex($"^{pickupCity}$", RegexOptions.IgnoreCase));

            criterias.Add(builder.Regex(r => r.PickupCity, queryExpr));
        }

        if (!string.IsNullOrWhiteSpace(dropOffCity))
        {
            var queryExpr = new BsonRegularExpression(new Regex($"^{dropOffCity}$", RegexOptions.IgnoreCase));

            criterias.Add(builder.Regex(r => r.DropOffCity, queryExpr));
        }

        if (onlyOpenOrComplete)
            criterias.Add(builder.In(r => r.Status, new List<RideStatus> { RideStatus.Open, RideStatus.Completed }));

        var filter = builder.And(criterias);

        return await _collection.Find(filter).ToListAsync();
    }
}
