using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Validator for GetSalesRequest
/// </summary>
public class GetSalesRequestValidator : AbstractValidator<GetSalesRequest>
{
    /// <summary>
    /// Initializes validation rules for GetSalesRequest
    /// </summary>
    public GetSalesRequestValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(request => request.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Size must not exceed 100");

        RuleFor(request => request.CustomerId)
            .MaximumLength(50)
            .WithMessage("Customer ID must not exceed 50 characters")
            .When(request => !string.IsNullOrEmpty(request.CustomerId));

        RuleFor(request => request.BranchId)
            .MaximumLength(50)
            .WithMessage("Branch ID must not exceed 50 characters")
            .When(request => !string.IsNullOrEmpty(request.BranchId));
    }
} 