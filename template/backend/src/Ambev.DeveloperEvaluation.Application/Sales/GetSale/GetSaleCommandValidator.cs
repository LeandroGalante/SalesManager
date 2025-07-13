using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for GetSaleCommand that defines validation rules for sale retrieval.
/// </summary>
public class GetSaleCommandValidator : AbstractValidator<GetSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the GetSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Id: Required, must not be empty
    /// </remarks>
    public GetSaleCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
} 