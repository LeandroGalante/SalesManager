using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common.MessageBroker;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMessageBrokerService _messageBrokerService;
    private readonly DeleteSaleHandler _handler;
    private readonly Faker _faker;

    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _messageBrokerService = Substitute.For<IMessageBrokerService>();
        _handler = new DeleteSaleHandler(_saleRepository, _messageBrokerService);
        _faker = new Faker();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldDeleteSaleSuccessfully()
    {
        // Arrange
        var saleId = _faker.Random.Guid();
        var existingSale = new Sale
        {
            Id = saleId,
            SaleNumber = _faker.Random.AlphaNumeric(10),
            CustomerId = _faker.Random.Guid().ToString(),
            CustomerName = _faker.Person.FullName,
            BranchId = _faker.Random.Guid().ToString(),
            BranchName = _faker.Company.CompanyName(),
            SaleDate = _faker.Date.Recent(),
            Items = new List<SaleItem>()
        };

        var command = new DeleteSaleCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(existingSale);

        _saleRepository.DeleteAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Message.Should().NotBeEmpty();

        await _saleRepository.Received(1)
            .GetByIdAsync(saleId, Arg.Any<CancellationToken>());
        
        await _saleRepository.Received(1)
            .UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        
        await _messageBrokerService.Received(1)
            .PublishAsync(Arg.Any<MediatR.INotification>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_SaleNotFound_ShouldReturnFailureResult()
    {
        // Arrange
        var saleId = _faker.Random.Guid();
        var command = new DeleteSaleCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("not found");
    }
} 