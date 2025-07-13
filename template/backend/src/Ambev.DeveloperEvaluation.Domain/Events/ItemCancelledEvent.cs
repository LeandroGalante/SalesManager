using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when a sale item is cancelled
/// </summary>
public class ItemCancelledEvent : INotification
{
    /// <summary>
    /// Gets the sale containing the cancelled item
    /// </summary>
    public Sale Sale { get; }
    
    /// <summary>
    /// Gets the item that was cancelled
    /// </summary>
    public SaleItem CancelledItem { get; }
    
    /// <summary>
    /// Gets the date and time when the event occurred
    /// </summary>
    public DateTime OccurredAt { get; }
    
    /// <summary>
    /// Gets the reason for cancellation
    /// </summary>
    public string? CancellationReason { get; }
    
    /// <summary>
    /// Initializes a new instance of the ItemCancelledEvent
    /// </summary>
    /// <param name="sale">The sale containing the cancelled item</param>
    /// <param name="cancelledItem">The item that was cancelled</param>
    /// <param name="cancellationReason">The reason for cancellation</param>
    public ItemCancelledEvent(Sale sale, SaleItem cancelledItem, string? cancellationReason = null)
    {
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
        CancelledItem = cancelledItem ?? throw new ArgumentNullException(nameof(cancelledItem));
        CancellationReason = cancellationReason;
        OccurredAt = DateTime.UtcNow;
    }
} 