namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Request model for creating a new sale
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch ID.
    /// </summary>
    public string BranchId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale date.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the items in the sale.
    /// </summary>
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}

/// <summary>
/// Request model for creating a sale item
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    public decimal UnitPrice { get; set; }
} 