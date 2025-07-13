namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Response model for DeleteSale operation
/// </summary>
public class DeleteSaleResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the sale was successfully deleted.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message describing the result of the delete operation.
    /// </summary>
    public string Message { get; set; } = string.Empty;
} 