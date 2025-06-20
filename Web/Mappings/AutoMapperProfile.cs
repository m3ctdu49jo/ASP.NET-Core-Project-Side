using AutoMapper;
using ShoppingMall.Web.DTOs;
using ShoppingMall.Web.Models;

namespace ShoppingMall.Web.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<User, UserDTO>()
                .ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Password)))
                .ForMember(dest => dest.LastLoginDate, opt => opt.Condition(src => !(src.LastLoginDate == null)));
        }
    }
} 