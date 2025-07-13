namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Specifies the possible statuses for a sale
/// </summary>
public enum SaleStatus
{
    /// <summary>
    /// Unknown status
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Sale is active and valid
    /// </summary>
    Active,
    
    /// <summary>
    /// Sale has been cancelled
    /// </summary>
    Cancelled
} 