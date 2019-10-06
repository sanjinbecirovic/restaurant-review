using AutoMapper;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.Web.Models.Response;

namespace RestaurantReview.Web.Models.Mapping_Profiles
{
    public class RestaurantResponseModel_MappingProfile : Profile
    {
        public RestaurantResponseModel_MappingProfile()
        {
            CreateMap<Restaurant, RestaurantResponseModel>().ReverseMap();
        }
    }
}
