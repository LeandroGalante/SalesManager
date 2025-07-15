using MediatR;

namespace Ambev.DeveloperEvaluation.Common.MessageBroker;

/// <summary>
/// Interface for message broker operations
/// </summary>
public interface IMessageBrokerService
{
    /// <summary>
    /// Publishes a domain event to the message broker
    /// </summary>
    /// <typeparam name="T">The type of the domain event</typeparam>
    /// <param name="domainEvent">The domain event to publish</param>
    /// <param name="topicName">The topic name to publish to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    Task PublishAsync<T>(T domainEvent, string topicName, CancellationToken cancellationToken = default) where T : INotification;
    
    /// <summary>
    /// Publishes a domain event to the message broker with default topic naming
    /// </summary>
    /// <typeparam name="T">The type of the domain event</typeparam>
    /// <param name="domainEvent">The domain event to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : INotification;
} 