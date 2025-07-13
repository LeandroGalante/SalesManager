using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Common.MessageBroker;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMessageBrokerService _messageBrokerService;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="messageBrokerService">The message broker service for publishing events</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IMessageBrokerService messageBrokerService)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _messageBrokerService = messageBrokerService;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Create the sale entity
        var sale = new Sale
        {
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            CustomerId = command.CustomerId,
            CustomerName = command.CustomerName,
            BranchId = command.BranchId,
            BranchName = command.BranchName
        };

        // Add items to the sale
        foreach (var itemCommand in command.Items)
        {
            var saleItem = new SaleItem
            {
                ProductId = itemCommand.ProductId,
                ProductName = itemCommand.ProductName,
                Quantity = itemCommand.Quantity,
                UnitPrice = itemCommand.UnitPrice
            };

            // Apply discount based on business rules
            saleItem.ApplyDiscount();

            sale.AddItem(saleItem);
        }

        // Validate the entire sale
        var saleValidation = sale.Validate();
        if (!saleValidation.IsValid)
        {
            var errorMessages = string.Join(", ", saleValidation.Errors.Select(e => e.Detail));
            throw new ValidationException($"Sale validation failed: {errorMessages}");
        }

        // Save the sale
        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        
        // Publish SaleCreated event to message broker
        await _messageBrokerService.PublishAsync(new SaleCreatedEvent(createdSale), cancellationToken);
        
        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }
} 