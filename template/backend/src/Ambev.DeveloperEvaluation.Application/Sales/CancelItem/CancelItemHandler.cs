using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Common.MessageBroker;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

/// <summary>
/// Handler for processing CancelItemCommand requests
/// </summary>
public class CancelItemHandler : IRequestHandler<CancelItemCommand, CancelItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBrokerService _messageBrokerService;

    /// <summary>
    /// Initializes a new instance of CancelItemHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="messageBrokerService">The message broker service for publishing events</param>
    public CancelItemHandler(ISaleRepository saleRepository, IMessageBrokerService messageBrokerService)
    {
        _saleRepository = saleRepository;
        _messageBrokerService = messageBrokerService;
    }

    /// <summary>
    /// Handles the CancelItemCommand request
    /// </summary>
    /// <param name="command">The CancelItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the item cancellation operation</returns>
    public async Task<CancelItemResult> Handle(CancelItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelItemCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        
        if (sale == null)
        {
            return new CancelItemResult
            {
                Success = false,
                Message = $"Sale with ID {command.SaleId} not found",
                SaleId = command.SaleId,
                ItemId = command.ItemId
            };
        }

        // Find the item to cancel
        var itemToCancel = sale.Items.FirstOrDefault(i => i.Id == command.ItemId);
        if (itemToCancel == null)
        {
            return new CancelItemResult
            {
                Success = false,
                Message = $"Item with ID {command.ItemId} not found in sale",
                SaleId = command.SaleId,
                ItemId = command.ItemId
            };
        }

        // Check if item is already cancelled
        if (itemToCancel.IsCancelled)
        {
            return new CancelItemResult
            {
                Success = false,
                Message = "Item is already cancelled",
                SaleId = command.SaleId,
                ItemId = command.ItemId
            };
        }

        // Cancel the item
        sale.CancelItem(command.ItemId);
        
        // Update the sale
        await _saleRepository.UpdateAsync(sale, cancellationToken);
        
        // Publish ItemCancelled event to message broker
        await _messageBrokerService.PublishAsync(new ItemCancelledEvent(sale, itemToCancel, command.CancellationReason), cancellationToken);

        return new CancelItemResult
        {
            Success = true,
            Message = "Item cancelled successfully",
            SaleId = command.SaleId,
            ItemId = command.ItemId
        };
    }
} 