using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.DeveloperEvaluation.Common.MessageBroker;

public record MessageDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; init; } = string.Empty;

    [BsonElement("messageId")]
    public Guid MessageId { get; init; } = Guid.NewGuid();

    [BsonElement("eventType")]
    public string EventType { get; init; } = string.Empty;

    [BsonElement("topicName")]
    public string TopicName { get; init; } = string.Empty;

    [BsonElement("messageBody")]
    public string MessageBody { get; init; } = string.Empty;

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    [BsonElement("processed")]
    public bool Processed { get; init; } = false;
} 