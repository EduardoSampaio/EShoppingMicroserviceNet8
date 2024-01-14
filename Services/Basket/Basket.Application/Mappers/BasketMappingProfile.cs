using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mappers;

public class BasketMappingProfile : Profile
{
    public BasketMappingProfile()
    {
        CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();
        CreateMap<ShoppingCartItem, ShoppingCartItemResponse>().ReverseMap();
        //CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        //CreateMap<BasketCheckoutV2, BasketCheckoutEventV2>().ReverseMap();
    }
}