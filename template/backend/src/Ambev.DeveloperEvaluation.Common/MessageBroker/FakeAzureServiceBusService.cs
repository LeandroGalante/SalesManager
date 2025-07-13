using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Common.MessageBroker;

/// <summary>
/// Fake implementation of Azure Service Bus service that logs messages to console
/// In a real scenario, this would publish messages to Azure Service Bus
/// </summary>
public class FakeAzureServiceBusService : IMessageBrokerService
{
    private readonly ILogger<FakeAzureServiceBusService> _logger;

    /// <summary>
    /// Initializes a new instance of the FakeAzureServiceBusService
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public FakeAzureServiceBusService(ILogger<FakeAzureServiceBusService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Publishes a domain event to Azure Service Bus (fake implementation - logs to console)
    /// </summary>
    /// <typeparam name="T">The type of the domain event</typeparam>
    /// <param name="domainEvent">The domain event to publish</param>
    /// <param name="topicName">The topic name to publish to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
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
            
            _logger.LogInformation(
                "📨 [Azure Service Bus] Publishing message to topic '{TopicName}' | Message ID: {MessageId} | Event Type: {EventType} | Message Body: {MessageBody}",
                topicName,
                messageId,
                typeof(T).Name,
                messageBody
            );

            // Simulate async operation
            await Task.Delay(10, cancellationToken);
            
            _logger.LogInformation(
                "✅ [Azure Service Bus] Successfully published message {MessageId} to topic '{TopicName}'",
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

    /// <summary>
    /// Publishes a domain event to Azure Service Bus with default topic naming
    /// </summary>
    /// <typeparam name="T">The type of the domain event</typeparam>
    /// <param name="domainEvent">The domain event to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    public async Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : INotification
    {
        // Generate topic name based on event type
        var topicName = $"sales-{typeof(T).Name.ToLowerInvariant().Replace("event", "")}";
        await PublishAsync(domainEvent, topicName, cancellationToken);
    }
} 