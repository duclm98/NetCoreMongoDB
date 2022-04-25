using Audit.Core;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NetCoreMongoDB.Entities;
using NetCoreMongoDB.Models;

namespace NetCoreMongoDB.Context;

public class DatabaseContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private const string Insert = "Insert";
    private const string Update = "Update";
    private const string Delete = "Delete";

    public DatabaseContext(IOptions<DatabaseSettings> dbOptions, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        var settings = dbOptions.Value;
        _client = new MongoClient(settings.ConnectionString);
        _database = _client.GetDatabase(settings.DatabaseName);
    }

    public MongoClient Client => _client;
    public IMongoDatabase Database => _database;

    public IMongoCollection<T> Collection<T>() where T : Base =>
        _database.GetCollection<T>(typeof(T).Name);

    public IFindFluent<T, T> Query<T>(FilterDefinition<T> filterDefinition = null) where T : Base
    {
        var newFilterDefinition = Builders<T>.Filter.Eq(x => x.DeletedAt, null);
        if (filterDefinition != null)
            newFilterDefinition &= filterDefinition;
        var mongoCollection = _database.GetCollection<T>(typeof(T).Name);
        return mongoCollection.Find(newFilterDefinition);
    }

    public async Task<T> FindAsync<T>(string id) where T : Base
    {
        var mongoCollection = _database.GetCollection<T>(typeof(T).Name);
        return await mongoCollection.Find(x => x.DeletedAt == null && x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task InsertAsync<T>(T entity) where T : Base
    {
        var collectionName = typeof(T).Name;
        var mongoCollection = _database.GetCollection<T>(collectionName);

        T entityNeedCompared = null;
        using var audit = await AuditScope.CreateAsync($"{collectionName}:{Insert}", () => entityNeedCompared);

        entity.Creator = await GetCreator();
        await mongoCollection.InsertOneAsync(entity);

        entityNeedCompared = entity;
        audit.Event.Target.Type = collectionName;
        audit.Event.CustomFields["CollectionName"] = collectionName;
        audit.Event.CustomFields["Action"] = Insert;
    }

    public async Task UpdateAsync<T>(T entity) where T : Base
    {
        var collectionName = typeof(T).Name;
        var mongoCollection = _database.GetCollection<T>(collectionName);

        var entityNeedCompared = await mongoCollection.Find(x => x.Id == entity.Id).FirstOrDefaultAsync();
        using var audit = await AuditScope.CreateAsync($"{collectionName}:{Update}", () => entityNeedCompared);

        entity.UpdatedAt = DateTime.Now;
        await mongoCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });

        entityNeedCompared = entity;
        audit.Event.Target.Type = collectionName;
        audit.Event.CustomFields["CollectionName"] = collectionName;
        audit.Event.CustomFields["Action"] = Update;
    }

    public async Task DeleteAsync<T>(T entity) where T : Base
    {
        var collectionName = typeof(T).Name;
        var mongoCollection = _database.GetCollection<T>(collectionName);

        using var audit = await AuditScope.CreateAsync($"{collectionName}:{Delete}", () => entity);

        entity.DeletedAt = DateTime.Now;
        await mongoCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });

        audit.Event.Target.Type = collectionName;
        audit.Event.CustomFields["CollectionName"] = collectionName;
        audit.Event.CustomFields["Action"] = Delete;
    }

    private async Task<Creator> GetCreator()
    {
        var httpContextUserId = _httpContextAccessor.HttpContext?.Items["userId"];
        if (httpContextUserId == null)
            return null;

        var user = await FindAsync<User>(httpContextUserId.ToString());
        if (user == null)
            return null;

        return new Creator
        {
            CreatorId = user.Id,
            CreatorName = user.Fullname,
            CreatorRole = user.Role
        };
    }
}