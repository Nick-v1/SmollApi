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
        }
    }
}
