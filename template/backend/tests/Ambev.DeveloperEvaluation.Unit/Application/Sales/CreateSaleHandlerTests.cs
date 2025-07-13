using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Common.MessageBroker;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMessageBrokerService _messageBrokerService;
    private readonly CreateSaleHandler _handler;
    private readonly Faker _faker;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _messageBrokerService = Substitute.For<IMessageBrokerService>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _messageBrokerService);
        _faker = new Faker();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateSaleSuccessfully()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = _faker.Random.AlphaNumeric(10),
            CustomerId = _faker.Random.Guid().ToString(),
            CustomerName = _faker.Person.FullName,
            BranchId = _faker.Random.Guid().ToString(),
            BranchName = _faker.Company.CompanyName(),
            SaleDate = _faker.Date.Recent(),
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductId = _faker.Random.Guid().ToString(),
                    ProductName = _faker.Commerce.ProductName(),
                    Quantity = _faker.Random.Int(1, 5),
                    UnitPrice = _faker.Random.Decimal(10, 100)
                }
            }
        };

        var createdSale = new Sale
        {
            Id = _faker.Random.Guid(),
            SaleNumber = command.SaleNumber,
            CustomerId = command.CustomerId,
            CustomerName = command.CustomerName,
            BranchId = command.BranchId,
            BranchName = command.BranchName,
            SaleDate = command.SaleDate
        };

        var expectedResult = new CreateSaleResult
        {
            Id = createdSale.Id,
            SaleNumber = createdSale.SaleNumber,
            TotalAmount = 0
        };

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(createdSale);

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedResult.Id);
        result.SaleNumber.Should().Be(expectedResult.SaleNumber);

        await _saleRepository.Received(1)
            .CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        
        await _messageBrokerService.Received(1)
            .PublishAsync(Arg.Any<MediatR.INotification>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var invalidCommand = new CreateSaleCommand
        {
            // Missing required fields to trigger validation error
            SaleNumber = "",
            CustomerId = "",
            Items = new List<CreateSaleItemCommand>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => _handler.Handle(invalidCommand, CancellationToken.None)
        );
    }
} 