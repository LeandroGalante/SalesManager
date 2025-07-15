namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Request model for getting paginated sales list
/// </summary>
public class GetSalesRequest
{
    /// <summary>
    /// Gets or sets the page number (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the customer ID filter (optional).
    /// </summary>
    public string? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch ID filter (optional).
    /// </summary>
    public string? BranchId { get; set; }

    /// <summary>
    /// Gets or sets the order by field and direction (optional).
    /// </summary>
    public string? OrderBy { get; set; }
} 