using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequest
{
    public string SaleNumber { get; init; } = string.Empty;
    public string CustomerId { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
    public string BranchId { get; init; } = string.Empty;
    public string BranchName { get; init; } = string.Empty;
    public DateTime SaleDate { get; init; }
    public List<CreateSaleItemRequest> Items { get; init; } = new();
}

public class CreateSaleItemRequest
{
    public string ProductId { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
} 