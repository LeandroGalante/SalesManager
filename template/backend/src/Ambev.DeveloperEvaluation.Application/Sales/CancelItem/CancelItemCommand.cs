using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

public class CancelItemCommand : ICommand<CancelItemResult>, IRequest<CancelItemResult>
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public string? CancellationReason { get; set; }
}

public class CancelItemResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid ItemId { get; set; }
    public Guid SaleId { get; set; }
}

public class CancelItemCommandValidator : AbstractValidator<CancelItemCommand>
{
    public CancelItemCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");
    }
} 