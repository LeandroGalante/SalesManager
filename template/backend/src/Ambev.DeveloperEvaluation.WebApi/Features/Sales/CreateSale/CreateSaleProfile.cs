using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Profile for mapping CreateSale requests and responses
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale feature
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();

        CreateMap<CreateSaleResult, CreateSaleResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<CreateSaleItemResult, CreateSaleItemResponse>();
    }
} 