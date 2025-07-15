using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchId { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public SaleStatus Status { get; set; }
    public decimal TotalAmount => Items?.Sum(item => item.TotalAmount) ?? 0;
    public virtual ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Sale()
    {
        Status = SaleStatus.Active;
        CreatedAt = DateTime.UtcNow;
        SaleDate = DateTime.UtcNow;
    }
    
    public void AddItem(SaleItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        
        item.SaleId = Id;
        item.ApplyDiscount();
        Items.Add(item);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void RemoveItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Items.Remove(item);
            UpdatedAt = DateTime.UtcNow;
        }
    }
    
    public void Cancel()
    {
        Status = SaleStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void CancelItem(Guid itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            item.Cancel();
            UpdatedAt = DateTime.UtcNow;
        }
    }
    
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