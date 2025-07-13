using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes validation rules for CreateSaleRequest
    /// </summary>
    public CreateSaleRequestValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .WithMessage("Sale number is required")
            .MaximumLength(50)
            .WithMessage("Sale number must not exceed 50 characters");

        RuleFor(sale => sale.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required")
            .MaximumLength(50)
            .WithMessage("Customer ID must not exceed 50 characters");

        RuleFor(sale => sale.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(200)
            .WithMessage("Customer name must not exceed 200 characters");

        RuleFor(sale => sale.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required")
            .MaximumLength(50)
            .WithMessage("Branch ID must not exceed 50 characters");

        RuleFor(sale => sale.BranchName)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .MaximumLength(200)
            .WithMessage("Branch name must not exceed 200 characters");

        RuleFor(sale => sale.SaleDate)
            .NotEmpty()
            .WithMessage("Sale date is required")
            .Must(date => date != default(DateTime))
            .WithMessage("Sale date must be valid");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("Sale must contain at least one item");

        RuleForEach(sale => sale.Items)
            .SetValidator(new CreateSaleItemRequestValidator());
    }
}

/// <summary>
/// Validator for CreateSaleItemRequest
/// </summary>
public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
{
    /// <summary>
    /// Initializes validation rules for CreateSaleItemRequest
    /// </summary>
    public CreateSaleItemRequestValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required")
            .MaximumLength(50)
            .WithMessage("Product ID must not exceed 50 characters");

        RuleFor(item => item.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(200)
            .WithMessage("Product name must not exceed 200 characters");

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