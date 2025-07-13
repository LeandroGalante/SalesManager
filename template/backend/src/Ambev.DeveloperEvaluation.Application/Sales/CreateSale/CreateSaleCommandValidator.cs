using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: Required, must not be empty, maximum length of 50 characters
    /// - CustomerId: Required, must not be empty, maximum length of 50 characters
    /// - CustomerName: Required, must not be empty, maximum length of 200 characters
    /// - BranchId: Required, must not be empty, maximum length of 50 characters
    /// - BranchName: Required, must not be empty, maximum length of 200 characters
    /// - SaleDate: Required, must not be default value
    /// - Items: Required, must contain at least one item
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required");

        RuleFor(sale => sale.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(sale => sale.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required");

        RuleFor(sale => sale.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required");

        RuleFor(sale => sale.BranchName)
            .NotEmpty()
            .WithMessage("Branch name is required");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty()
            .WithMessage("Sale date is required")
            .Must(date => date != default(DateTime))
            .WithMessage("Sale date must be valid");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("Sale must contain at least one item");

        RuleForEach(sale => sale.Items)
            .SetValidator(new CreateSaleItemCommandValidator());
    }
}

/// <summary>
/// Validator for CreateSaleItemCommand that defines validation rules for sale item creation.
/// </summary>
public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleItemCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - ProductId: Required, must not be empty, maximum length of 50 characters
    /// - ProductName: Required, must not be empty, maximum length of 200 characters
    /// - Quantity: Required, must be greater than 0 and not exceed 20
    /// - UnitPrice: Required, must be greater than 0
    /// </remarks>
    public CreateSaleItemCommandValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(item => item.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than 0");
    }
} 