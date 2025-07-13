namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Request model for getting a sale by ID
/// </summary>
public class GetSaleRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to retrieve.
    /// </summary>
    public Guid Id { get; set; }
} 