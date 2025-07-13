using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Common.MessageBroker;

public class FakeAzureServiceBusService : IMessageBrokerService
{
    private readonly ILogger<FakeAzureServiceBusService> _logger;
    private readonly IMongoDbService _mongoDbService;

    public FakeAzureServiceBusService(ILogger<FakeAzureServiceBusService> logger, IMongoDbService mongoDbService)
    {
        _logger = logger;
        _mongoDbService = mongoDbService;
    }

    public async Task PublishAsync<T>(T domainEvent, string topicName, CancellationToken cancellationToken = default) where T : INotification
    {
        try
        {
            var messageId = Guid.NewGuid();
            var messageBody = JsonSerializer.Serialize(domainEvent, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
            
            // Save message to MongoDB (simulated)
            var messageDocument = new MessageDocument
            {
                MessageId = messageId,
                EventType = typeof(T).Name,
                TopicName = topicName,
                MessageBody = messageBody,
                Processed = false
            };
            
            var documentId = await _mongoDbService.InsertMessageAsync(messageDocument, cancellationToken);
            
            _logger.LogInformation(
                "📨 [Azure Service Bus] Publishing message to topic '{TopicName}' | Message ID: {MessageId} | Event Type: {EventType} | MongoDB Document ID: {DocumentId}",
                topicName,
                messageId,
                typeof(T).Name,
                documentId
            );

            // Simulate async operation
            await Task.Delay(10, cancellationToken);
            
            _logger.LogInformation(
                "✅ [Azure Service Bus] Successfully published message {MessageId} to topic '{TopicName}' and saved to MongoDB",
                messageId,
                topicName
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "❌ [Azure Service Bus] Failed to publish message to topic '{TopicName}' | Event Type: {EventType}",
                topicName,
                typeof(T).Name
            );
            throw;
        }
    }

    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : INotification
    {
        // Generate topic name based on event type
        var topicName = $"sales-{typeof(T).Name.ToLowerInvariant().Replace("event", "")}";
        await PublishAsync(domainEvent, topicName, cancellationToken);
    }
} 