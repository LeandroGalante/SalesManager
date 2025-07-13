using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Response model for GetSales operation with pagination
/// </summary>
public class GetSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales.
    /// </summary>
    public List<GetSaleResult> Data { get; set; } = new();

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of items.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;
} 