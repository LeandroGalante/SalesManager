namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItem;

/// <summary>
/// Request model for cancelling an item in a sale
/// </summary>
public class CancelItemRequest
{
    /// <summary>
    /// Gets or sets the reason for cancellation
    /// </summary>
    public string? CancellationReason { get; set; }
} 