using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelItem;

/// <summary>
/// AutoMapper profile for CancelItem feature
/// </summary>
public class CancelItemProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CancelItem
    /// </summary>
    public CancelItemProfile()
    {
        CreateMap<CancelItemResult, CancelItemResponse>();
    }
} 