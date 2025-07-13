using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Sale entity and CreateSale operation DTOs
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateSale operation
    /// </summary>
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore());

        CreateMap<CreateSaleItemCommand, SaleItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SaleId, opt => opt.Ignore())
            .ForMember(dest => dest.Discount, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancelled, opt => opt.Ignore())
            .ForMember(dest => dest.Sale, opt => opt.Ignore());

        CreateMap<Sale, CreateSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

        CreateMap<SaleItem, CreateSaleItemResult>()
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
    }
} 