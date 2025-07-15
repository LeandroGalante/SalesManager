namespace Ambev.DeveloperEvaluation.Common.MessageBroker;

public interface IMongoDbService
{
    Task<string> InsertMessageAsync(MessageDocument message, CancellationToken cancellationToken = default);
    Task<IEnumerable<MessageDocument>> GetMessagesAsync(string? eventType = null, CancellationToken cancellationToken = default);
    Task<MessageDocument?> GetMessageByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> MarkAsProcessedAsync(string id, CancellationToken cancellationToken = default);
} 