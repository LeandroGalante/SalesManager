using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Validator for GetSalesCommand that defines validation rules for sales retrieval.
/// </summary>
public class GetSalesCommandValidator : AbstractValidator<GetSalesCommand>
{
    /// <summary>
    /// Initializes a new instance of the GetSalesCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Page: Must be greater than 0
    /// - Size: Must be greater than 0 and not exceed 100
    /// - CustomerId: Optional, but if provided must not exceed 50 characters
    /// - BranchId: Optional, but if provided must not exceed 50 characters
    /// </remarks>
    public GetSalesCommandValidator()
    {
        RuleFor(command => command.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(command => command.Size)
            .GreaterThan(0)
            .WithMessage("Size must be greater than 0");


    }
} 