using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command for updating an existing sale.
/// </summary>
/// <remarks>
/// This command is used to update a sale with new information.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="UpdateSaleResult"/>.
/// </remarks>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch ID.
    /// </summary>
    public string BranchId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the items in the sale.
    /// </summary>
    public List<UpdateSaleItemCommand> Items { get; set; } = new();

    /// <summary>
    /// Validates the command using FluentValidation.
    /// </summary>
    /// <returns>Validation result with any errors</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new UpdateSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

/// <summary>
/// Command for updating a sale item within a sale.
/// </summary>
public class UpdateSaleItemCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale item (optional for new items).
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    public decimal UnitPrice { get; set; }
} 