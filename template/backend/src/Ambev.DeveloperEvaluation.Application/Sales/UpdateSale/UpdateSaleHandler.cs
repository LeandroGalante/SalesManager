using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Common.MessageBroker;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMessageBrokerService _messageBrokerService;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="messageBrokerService">The message broker service for publishing events</param>
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IMessageBrokerService messageBrokerService)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _messageBrokerService = messageBrokerService;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new InvalidOperationException($"Sale with ID {command.Id} not found");

        // Update sale properties
        sale.CustomerId = command.CustomerId;
        sale.CustomerName = command.CustomerName;
        sale.BranchId = command.BranchId;
        sale.BranchName = command.BranchName;

        // Clear existing items and add new ones
        sale.Items.Clear();
        
        foreach (var itemCommand in command.Items)
        {
            var saleItem = new SaleItem
            {
                ProductId = itemCommand.ProductId,
                ProductName = itemCommand.ProductName,
                Quantity = itemCommand.Quantity,
                UnitPrice = itemCommand.UnitPrice
            };

            // If it's an existing item, preserve the ID
            if (itemCommand.Id.HasValue)
            {
                saleItem.Id = itemCommand.Id.Value;
            }

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

        // Save the updated sale
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        
        // Publish SaleModified event to message broker
        await _messageBrokerService.PublishAsync(new SaleModifiedEvent(updatedSale), cancellationToken);
        
        var result = _mapper.Map<UpdateSaleResult>(updatedSale);
        return result;
    }
} 