using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Command for retrieving a paginated list of sales.
/// </summary>
/// <remarks>
/// This command is used to fetch sales with optional filtering and pagination.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="GetSalesResult"/>.
/// </remarks>
public class GetSalesCommand : IRequest<GetSalesResult>
{
    /// <summary>
    /// Gets or sets the page number (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the customer ID filter (optional).
    /// </summary>
    public string? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the branch ID filter (optional).
    /// </summary>
    public string? BranchId { get; set; }

    /// <summary>
    /// Gets or sets the order by field and direction (optional).
    /// Example: "saleDate desc", "totalAmount asc"
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <returns>Validation result with any errors</returns>
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