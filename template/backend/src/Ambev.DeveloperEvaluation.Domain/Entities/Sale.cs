using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale number.
    /// Must be unique across all sales.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }
    
    /// <summary>
    /// Gets or sets the customer identifier.
    /// Using External Identities pattern for referencing customers from other domains.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer name.
    /// Denormalized from the customer domain for performance.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch identifier where the sale was made.
    /// Using External Identities pattern for referencing branches from other domains.
    /// </summary>
    public string BranchId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch name.
    /// Denormalized from the branch domain for performance.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the sale status.
    /// Indicates whether the sale is active or cancelled.
    /// </summary>
    public SaleStatus Status { get; set; }
    
    /// <summary>
    /// Gets the total amount of the sale.
    /// Calculated as the sum of all item total amounts.
    /// </summary>
    public decimal TotalAmount => Items?.Sum(item => item.TotalAmount) ?? 0;
    
    /// <summary>
    /// Gets or sets the collection of items in this sale.
    /// </summary>
    public virtual ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    
    /// <summary>
    /// Gets or sets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time of the last update to the sale.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        Status = SaleStatus.Active;
        CreatedAt = DateTime.UtcNow;
        SaleDate = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Adds an item to the sale.
    /// </summary>
    /// <param name="item">The item to add</param>
    public void AddItem(SaleItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        
        item.SaleId = Id;
        item.ApplyDiscount();
        Items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Removes an item from the sale.
    /// </summary>
    /// <param name="itemId">The ID of the item to remove</param>
    public void RemoveItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Items.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }
    
    /// <summary>
    /// Cancels the entire sale.
    /// </summary>
    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Cancels a specific item in the sale.
    /// </summary>
    /// <param name="itemId">The ID of the item to cancel</param>
    public void CancelItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            item.Cancel();
            UpdatedAt = DateTime.UtcNow;
        }
    }
    
    /// <summary>
    /// Validates the sale according to business rules.
    /// </summary>
    /// <returns>Validation result with any errors</returns>
    public ValidationResultDetail Validate()
    {
        var errors = new List<ValidationErrorDetail>();
        
        if (string.IsNullOrWhiteSpace(SaleNumber))
            errors.Add(new ValidationErrorDetail { Error = "SaleNumber", Detail = "Sale number is required" });
        
        if (string.IsNullOrWhiteSpace(CustomerId))
            errors.Add(new ValidationErrorDetail { Error = "CustomerId", Detail = "Customer ID is required" });
        
        if (string.IsNullOrWhiteSpace(CustomerName))
            errors.Add(new ValidationErrorDetail { Error = "CustomerName", Detail = "Customer name is required" });
        
        if (string.IsNullOrWhiteSpace(BranchId))
            errors.Add(new ValidationErrorDetail { Error = "BranchId", Detail = "Branch ID is required" });
        
        if (string.IsNullOrWhiteSpace(BranchName))
            errors.Add(new ValidationErrorDetail { Error = "BranchName", Detail = "Branch name is required" });
        
        if (SaleDate == default)
            errors.Add(new ValidationErrorDetail { Error = "SaleDate", Detail = "Sale date is required" });
        
        if (Items == null || !Items.Any())
            errors.Add(new ValidationErrorDetail { Error = "Items", Detail = "Sale must contain at least one item" });
        
        // Validate all items
        if (Items != null)
        {
            foreach (var item in Items)
            {
                var itemValidation = item.Validate();
                if (!itemValidation.IsValid)
                {
                    errors.AddRange(itemValidation.Errors);
                }
            }
        }
        
        return new ValidationResultDetail
        {
            IsValid = !errors.Any(),
            Errors = errors
        };
    }
} 