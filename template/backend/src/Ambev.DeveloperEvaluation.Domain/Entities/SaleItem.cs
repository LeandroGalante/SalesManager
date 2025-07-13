using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount => (Quantity * UnitPrice) * (1 - Discount / 100);
    public bool IsCancelled { get; set; }
    
    [JsonIgnore]
    public virtual Sale Sale { get; set; } = null!;
    
    public SaleItem()
    {
        IsCancelled = false;
    }
    
    public decimal CalculateDiscountPercentage()
    {
        if (Quantity < 4)
            return 0; // No discount for less than 4 items
        
        if (Quantity >= 4 && Quantity < 10)
            return 10; // 10% discount for 4-9 items
        
        if (Quantity >= 10 && Quantity <= 20)
            return 20; // 20% discount for 10-20 items
        
        throw new InvalidOperationException("Cannot sell more than 20 identical items");
    }
    
    public void ApplyDiscount()
    {
        if (Quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 identical items");
        
        Discount = CalculateDiscountPercentage();
    }
    
    public void Cancel()
    {
        IsCancelled = true;
    }
    
    public ValidationResultDetail Validate()
    {
        var errors = new List<ValidationErrorDetail>();
        
        if (string.IsNullOrWhiteSpace(ProductId))
            errors.Add(new ValidationErrorDetail { Error = "ProductId", Detail = "Product ID is required" });
        
        if (string.IsNullOrWhiteSpace(ProductName))
            errors.Add(new ValidationErrorDetail { Error = "ProductName", Detail = "Product name is required" });
        
        if (Quantity <= 0)
            errors.Add(new ValidationErrorDetail { Error = "Quantity", Detail = "Quantity must be greater than 0" });
        
        if (Quantity > 20)
            errors.Add(new ValidationErrorDetail { Error = "Quantity", Detail = "Cannot sell more than 20 identical items" });
        
        if (UnitPrice <= 0)
            errors.Add(new ValidationErrorDetail { Error = "UnitPrice", Detail = "Unit price must be greater than 0" });
        
        if (Discount < 0 || Discount > 100)
            errors.Add(new ValidationErrorDetail { Error = "Discount", Detail = "Discount must be between 0 and 100" });
        
        return new ValidationResultDetail
        {
            IsValid = !errors.Any(),
            Errors = errors
        };
    }
} 