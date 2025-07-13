using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Validator for DeleteSaleCommand that defines validation rules for sale deletion.
/// </summary>
public class DeleteSaleCommandValidator : AbstractValidator<DeleteSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the DeleteSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Id: Required, must not be empty
    /// </remarks>
    public DeleteSaleCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
} 