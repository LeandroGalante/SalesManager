using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event raised when a sale is cancelled
/// </summary>
public class SaleCancelledEvent : INotification
{
    /// <summary>
    /// Gets the sale that was cancelled
    /// </summary>
    public Sale Sale { get; }
    
    /// <summary>
    /// Gets the date and time when the event occurred
    /// </summary>
    public DateTime OccurredAt { get; }
    
    /// <summary>
    /// Gets the reason for cancellation
    /// </summary>
    public string? CancellationReason { get; }
    
    /// <summary>
    /// Initializes a new instance of the SaleCancelledEvent
    /// </summary>
    /// <param name="sale">The sale that was cancelled</param>
    /// <param name="cancellationReason">The reason for cancellation</param>
    public SaleCancelledEvent(Sale sale, string? cancellationReason = null)
    {
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
        CancellationReason = cancellationReason;
        OccurredAt = DateTime.UtcNow;
    }
} 