using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequest
{
    public Guid CustomerId { get; init; }
    public string Branch { get; init; } = string.Empty;
    public List<CreateSaleItemRequest> Items { get; init; } = new();
}

public class CreateSaleItemRequest
{
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
} 