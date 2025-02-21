using AutoMapper;
using WebShop.Domain.Models;
using WebShopAPI.DTOs;
namespace WebShopAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDTO, User>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => new List<Order>()))
                .ForMember(dest => dest.Role, opt => opt.Ignore()) 
                .ForMember(dest => dest.Password, opt => opt.Ignore()); 

            CreateMap<Product, ProductResponseDTO>();

            CreateMap<Order, OrderResponseDTO>();

            CreateMap<OrderRequestDTO, Order>();

            CreateMap<OrderUpdateDTO, Order>();

            CreateMap<OrderProductRequestedDTO, OrderProductRequested>()
            .ForMember(dest => dest.OrderId, opt => opt.Ignore());

            CreateMap<OrderProductRequested, OrderProductResponseDTO>();
        }
    }
}
