
using AutoMapper;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.Web.Models.Response;

namespace RestaurantReview.Web.Models.Mapping_Profiles
{
    public class UserResponseModel_MappingProfile : Profile
    {
        public UserResponseModel_MappingProfile()
        {
            CreateMap<User, UserResponseModel>()
                .ForMember(u => u.Email, m => m.MapFrom(u => u.Email))
                .ForMember(u => u.Id, m => m.MapFrom(u => u.Id))
                .ForMember(u => u.Username, m => m.MapFrom(u => u.UserName))
                .ForMember(u => u.LockoutEnd, m => m.MapFrom(u => u.LockoutEnd))
                .ReverseMap();
        }
    }
}
