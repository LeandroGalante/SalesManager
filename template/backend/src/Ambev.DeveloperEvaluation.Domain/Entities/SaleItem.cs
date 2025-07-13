using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item within a sale transaction.
/// This entity contains product information, quantities, pricing, and discount details.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the sale this item belongs to.
    /// </summary>
    public Guid SaleId { get; set; }
    
    /// <summary>
    /// Gets or sets the product identifier.
    /// Using External Identities pattern for referencing products from other domains.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the product name (denormalized for performance).
    /// </summary>
    public string ProductName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the quantity of the product in this sale.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the discount percentage applied to this item (0-100).
    /// </summary>
    public decimal Discount { get; set; }
    
    /// <summary>
    /// Gets the total amount for this item after applying discount.
    /// </summary>
    public decimal TotalAmount => (Quantity * UnitPrice) * (1 - Discount / 100);
    
    /// <summary>
    /// Gets or sets whether this item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
    
    /// <summary>
    /// Navigation property to the sale this item belongs to.
    /// </summary>
    [JsonIgnore]
    public virtual Sale Sale { get; set; } = null!;
    
    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    public SaleItem()
    {
        IsCancelled = false;
    }
    
    /// <summary>
    /// Calculates the appropriate discount based on quantity business rules.
    /// </summary>
    /// <returns>The discount percentage to apply</returns>
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
    
    /// <summary>
    /// Applies the discount based on quantity business rules.
    /// </summary>
    public void ApplyDiscount()
    {
        if (Quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 identical items");
        
        Discount = CalculateDiscountPercentage();
    }
    
    /// <summary>
    /// Cancels this item.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
    }
    
    /// <summary>
    /// Validates the sale item according to business rules.
    /// </summary>
    /// <returns>Validation result with any errors</returns>
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