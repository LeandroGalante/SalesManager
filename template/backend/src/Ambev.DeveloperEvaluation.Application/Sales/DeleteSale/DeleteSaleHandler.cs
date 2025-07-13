using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Common.MessageBroker;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for processing DeleteSaleCommand requests
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBrokerService _messageBrokerService;

    /// <summary>
    /// Initializes a new instance of DeleteSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="messageBrokerService">The message broker service for publishing events</param>
    public DeleteSaleHandler(ISaleRepository saleRepository, IMessageBrokerService messageBrokerService)
    {
        _saleRepository = saleRepository;
        _messageBrokerService = messageBrokerService;
    }

    /// <summary>
    /// Handles the DeleteSaleCommand request
    /// </summary>
    /// <param name="command">The DeleteSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the delete operation</returns>
    public async Task<DeleteSaleResult> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new DeleteSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        
        if (sale == null)
        {
            return new DeleteSaleResult
            {
                Success = false,
                Message = $"Sale with ID {command.Id} not found"
            };
        }

        // Cancel the sale instead of deleting it
        sale.Cancel();
        
        // Update the sale with cancelled status
        await _saleRepository.UpdateAsync(sale, cancellationToken);
        
        // Publish SaleCancelled event to message broker
        await _messageBrokerService.PublishAsync(new SaleCancelledEvent(sale, "Sale cancelled via delete operation"), cancellationToken);

        return new DeleteSaleResult
        {
            Success = true,
            Message = "Sale cancelled successfully"
        };
    }
} 