using AutoMapper;
using SmollApi.Models.Dtos;

namespace SmollApi.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserSearchDto>();
            CreateMap<User, UserAccountManagement>();
            CreateMap<User, UserDtoDetails>();
            CreateMap<UserAccountManagement, User>();
            CreateMap<UserDtoDetails, User>();
            //----------------------------------------
            CreateMap<Phone, PhoneDto>();
            CreateMap<PhoneDto, Phone>();
            //----------------------------------------
            CreateMap<BanDto, Ban>();
            CreateMap<Ban, BanDtoResult>();
            CreateMap<BanDtoResult, Ban>();
            CreateMap<UnbanDto, Ban>();
            //----------------------------------------
            CreateMap<Favourite, FavouriteDto>();
            CreateMap<FavouriteDto, Favourite>();
            //----------------------------------------
            CreateMap<Product, ProductCREATEDto>();
            CreateMap<ProductCREATEDto, Product>();
            //----------------------------------------
            CreateMap<OrderDto, Order>();
        }
    }
}
