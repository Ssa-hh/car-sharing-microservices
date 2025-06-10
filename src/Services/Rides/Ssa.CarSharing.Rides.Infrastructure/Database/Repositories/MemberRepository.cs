using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ssa.CarSharing.Rides.Domain.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Rides.Infrastructure.Database.Repositories;

internal class MemberRepository : IMemberRepository
{
    private readonly IMongoCollection<Member> _collection;

    public MemberRepository(IMongoClient mongoClient, IOptions<MongoDbOptions> options)
    {

        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<Member>(options.Value.MemberCollectionName);
    }

    public async Task AddAsync(Member member, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(member);
    }
    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken)
    {
        FilterDefinition<Member> filter = Builders<Member>.Filter.Eq(m => m.Email, email);

        var count = await _collection.CountDocumentsAsync(filter);

        return count > 0;
    }

    public async Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        FilterDefinition<Member> filter = Builders<Member>.Filter.Eq(m=>m.Email, email);

        Member? member = await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);

        return member;
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        FilterDefinition<Member> filter = Builders<Member>.Filter.Eq(m => m.Id, id);

        Member? member = await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);

        return member;
    }
}
