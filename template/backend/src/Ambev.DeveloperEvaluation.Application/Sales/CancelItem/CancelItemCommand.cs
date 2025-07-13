using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Command for cancelling a specific item in a sale
/// </summary>
public class CancelItemCommand : IRequest<CancelItemResult>
{
    /// <summary>
    /// Gets or sets the sale ID
    /// </summary>
    public Guid SaleId { get; set; }
    
    /// <summary>
    /// Gets or sets the item ID to cancel
    /// </summary>
    public Guid ItemId { get; set; }
    
    /// <summary>
    /// Gets or sets the reason for cancellation
    /// </summary>
    public string? CancellationReason { get; set; }
} 