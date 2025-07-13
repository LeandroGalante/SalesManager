namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Result for item cancellation operation
/// </summary>
public class CancelItemResult
{
    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Gets or sets the operation message
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the ID of the cancelled item
    /// </summary>
    public Guid ItemId { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the sale containing the item
    /// </summary>
    public Guid SaleId { get; set; }
} 