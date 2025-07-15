using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when a new sale is created
/// </summary>
public class SaleCreatedEvent : INotification
{
    /// <summary>
    /// Gets the sale that was created
    /// </summary>
    public Sale Sale { get; }
    
    /// <summary>
    /// Gets the date and time when the event occurred
    /// </summary>
    public DateTime OccurredAt { get; }
    
    /// <summary>
    /// Initializes a new instance of the SaleCreatedEvent
    /// </summary>
    /// <param name="sale">The sale that was created</param>
    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
        OccurredAt = DateTime.UtcNow;
    }
} 