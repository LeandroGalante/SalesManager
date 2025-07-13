using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesCommand requests
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSalesHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSalesCommand request
    /// </summary>
    /// <param name="command">The GetSales command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The paginated sales result</returns>
    public async Task<GetSalesResult> Handle(GetSalesCommand command, CancellationToken cancellationToken)
    {
        var validator = new GetSalesCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Get sales based on filters
        IEnumerable<Domain.Entities.Sale> sales;
        
        if (!string.IsNullOrEmpty(command.CustomerId))
        {
            sales = await _saleRepository.GetByCustomerIdAsync(command.CustomerId, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(command.BranchId))
        {
            sales = await _saleRepository.GetByBranchIdAsync(command.BranchId, cancellationToken);
        }
        else
        {
            sales = await _saleRepository.GetAllAsync(command.Page, command.Size, cancellationToken);
        }

        // Get total count for pagination
        var totalCount = await _saleRepository.GetTotalCountAsync(cancellationToken);

        // Apply additional filtering if multiple filters are specified
        if (!string.IsNullOrEmpty(command.CustomerId) && !string.IsNullOrEmpty(command.BranchId))
        {
            sales = sales.Where(s => s.CustomerId == command.CustomerId && s.BranchId == command.BranchId);
        }

        // Apply sorting if specified
        if (!string.IsNullOrEmpty(command.OrderBy))
        {
            sales = ApplySorting(sales, command.OrderBy);
        }

        // Apply pagination if not already applied by repository
        if (string.IsNullOrEmpty(command.CustomerId) && string.IsNullOrEmpty(command.BranchId))
        {
            // Already paginated by repository
        }
        else
        {
            // Apply pagination to filtered results
            sales = sales.Skip((command.Page - 1) * command.Size).Take(command.Size);
        }

        var salesList = sales.ToList();
        var totalPages = (int)Math.Ceiling((double)totalCount / command.Size);

        var result = new GetSalesResult
        {
            Data = _mapper.Map<List<GetSaleResult>>(salesList),
            CurrentPage = command.Page,
            PageSize = command.Size,
            TotalItems = totalCount,
            TotalPages = totalPages
        };

        return result;
    }

    /// <summary>
    /// Applies sorting to the sales collection based on the order by parameter
    /// </summary>
    /// <param name="sales">The sales collection</param>
    /// <param name="orderBy">The order by parameter</param>
    /// <returns>The sorted sales collection</returns>
    private static IEnumerable<Domain.Entities.Sale> ApplySorting(IEnumerable<Domain.Entities.Sale> sales, string orderBy)
    {
        var orderParts = orderBy.Split(' ');
        var field = orderParts[0].ToLower();
        var direction = orderParts.Length > 1 ? orderParts[1].ToLower() : "asc";

        return field switch
        {
            "saledate" => direction == "desc" 
                ? sales.OrderByDescending(s => s.SaleDate)
                : sales.OrderBy(s => s.SaleDate),
            "totalamount" => direction == "desc"
                ? sales.OrderByDescending(s => s.TotalAmount)
                : sales.OrderBy(s => s.TotalAmount),
            "customername" => direction == "desc"
                ? sales.OrderByDescending(s => s.CustomerName)
                : sales.OrderBy(s => s.CustomerName),
            "branchname" => direction == "desc"
                ? sales.OrderByDescending(s => s.BranchName)
                : sales.OrderBy(s => s.BranchName),
            "salenumber" => direction == "desc"
                ? sales.OrderByDescending(s => s.SaleNumber)
                : sales.OrderBy(s => s.SaleNumber),
            _ => sales.OrderByDescending(s => s.SaleDate) // Default sorting
        };
    }
} 