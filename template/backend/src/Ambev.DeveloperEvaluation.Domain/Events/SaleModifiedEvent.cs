using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when a sale is modified
/// </summary>
public class SaleModifiedEvent : INotification
{
    /// <summary>
    /// Gets the sale that was modified
    /// </summary>
    public Sale Sale { get; }
    
    /// <summary>
    /// Gets the date and time when the event occurred
    /// </summary>
    public DateTime OccurredAt { get; }
    
    /// <summary>
    /// Initializes a new instance of the SaleModifiedEvent
    /// </summary>
    /// <param name="sale">The sale that was modified</param>
    public SaleModifiedEvent(Sale sale)
    {
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
        OccurredAt = DateTime.UtcNow;
    }
} 