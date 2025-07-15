using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleResult
{
    public Guid Id { get; init; }
    public string SaleNumber { get; init; } = string.Empty;
    public Guid CustomerId { get; init; }
    public string Branch { get; init; } = string.Empty;
    public DateTime SaleDate { get; init; }
    public decimal TotalAmount { get; init; }
    public SaleStatus Status { get; init; }
    public List<CreateSaleItemResult> Items { get; init; } = new();
}

public class CreateSaleItemResult
{
    public Guid Id { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal Discount { get; init; }
    public decimal TotalPrice { get; init; }
    public bool IsCancelled { get; init; }
} 