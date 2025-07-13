using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Profile for mapping GetSales requests and responses
/// </summary>
public class GetSalesProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSales feature
    /// </summary>
    public GetSalesProfile()
    {
        CreateMap<GetSalesRequest, GetSalesCommand>();

        CreateMap<GetSalesResult, GetSalesResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
    }
} 