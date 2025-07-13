using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Command for deleting a sale.
/// </summary>
/// <remarks>
/// This command is used to delete a sale by its unique identifier.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="DeleteSaleResult"/>.
/// </remarks>
public class DeleteSaleCommand : IRequest<DeleteSaleResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to delete.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the DeleteSaleCommand class.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete.</param>
    public DeleteSaleCommand(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <returns>Validation result with any errors</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new DeleteSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
} 