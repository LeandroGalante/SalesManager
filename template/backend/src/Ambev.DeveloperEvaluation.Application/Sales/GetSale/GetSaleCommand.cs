using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public class GetSaleCommand : ICommand<GetSaleResult>, IRequest<GetSaleResult>
{
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the GetSaleCommand class.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to retrieve.</param>
    public GetSaleCommand(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <returns>Validation result with any errors</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new GetSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
} 