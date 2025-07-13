using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Profile for mapping UpdateSale requests and responses
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale feature
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // ID is set from route

        CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();

        CreateMap<UpdateSaleResult, UpdateSaleResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
    }
} 