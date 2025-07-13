using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace Ambev.DeveloperEvaluation.Common.MessageBroker;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<MessageDocument> _collection;

    public MongoDbService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("message_broker");
        _collection = _database.GetCollection<MessageDocument>("messages");
    }

    public async Task<string> InsertMessageAsync(MessageDocument message, CancellationToken cancellationToken = default)
    {
        var messageWithId = string.IsNullOrEmpty(message.Id) 
            ? message with { Id = Guid.NewGuid().ToString() }
            : message;
        
        await _collection.InsertOneAsync(messageWithId, cancellationToken: cancellationToken);
        return messageWithId.Id;
    }

    public async Task<IEnumerable<MessageDocument>> GetMessagesAsync(string? eventType = null, CancellationToken cancellationToken = default)
    {
        var filter = eventType != null ? 
            Builders<MessageDocument>.Filter.Eq(x => x.EventType, eventType) : 
            Builders<MessageDocument>.Filter.Empty;
            
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<MessageDocument?> GetMessageByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> MarkAsProcessedAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<MessageDocument>.Filter.Eq(x => x.Id, id);
        var update = Builders<MessageDocument>.Update.Set(x => x.Processed, true);
        
        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }
} 