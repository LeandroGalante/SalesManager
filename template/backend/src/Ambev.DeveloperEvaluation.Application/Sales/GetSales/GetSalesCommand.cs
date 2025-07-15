using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

public class GetSalesCommand : ICommand<GetSalesResult>, IRequest<GetSalesResult>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? CustomerId { get; set; }
    public string? BranchId { get; set; }
    public string? OrderBy { get; set; }

    public ValidationResultDetail Validate()
    {
        var validator = new GetSalesCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

public class GetSalesResult
{
    public List<GetSaleResult> Data { get; set; } = new();
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}

public class GetSalesCommandValidator : AbstractValidator<GetSalesCommand>
{
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