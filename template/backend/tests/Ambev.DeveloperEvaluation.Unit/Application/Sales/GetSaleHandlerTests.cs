using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;
    private readonly Faker _faker;

    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
        _faker = new Faker();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSaleSuccessfully()
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

        var command = new GetSaleCommand(saleId);

        var expectedResult = new GetSaleResult
        {
            Id = saleId,
            SaleNumber = existingSale.SaleNumber,
            CustomerId = existingSale.CustomerId,
            CustomerName = existingSale.CustomerName,
            TotalAmount = 0
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns(existingSale);

        _mapper.Map<GetSaleResult>(existingSale)
            .Returns(expectedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedResult.Id);
        result.SaleNumber.Should().Be(expectedResult.SaleNumber);
        result.CustomerId.Should().Be(expectedResult.CustomerId);

        await _saleRepository.Received(1)
            .GetByIdAsync(saleId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_SaleNotFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var saleId = _faker.Random.Guid();
        var command = new GetSaleCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }
} 