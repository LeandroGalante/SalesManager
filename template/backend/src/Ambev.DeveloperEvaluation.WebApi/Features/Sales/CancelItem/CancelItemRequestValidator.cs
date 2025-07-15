using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItem;

/// <summary>
/// Validator for CancelItemRequest
/// </summary>
public class CancelItemRequestValidator : AbstractValidator<CancelItemRequest>
{
    /// <summary>
    /// Initializes validation rules for CancelItemRequest
    /// </summary>
    public CancelItemRequestValidator()
    {
        RuleFor(x => x.CancellationReason)
            .MaximumLength(500)
            .WithMessage("Cancellation reason must not exceed 500 characters");
    }
} 