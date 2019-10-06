using AutoMapper;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.Web.Models.Response;

namespace RestaurantReview.Web.Models.Mapping_Profiles
{
    public class ReviewResponseModel_MappingProfile : Profile
    {
        public ReviewResponseModel_MappingProfile()
        {
            CreateMap<Review, ReviewsResponseModel>().ReverseMap();
        }
    }
}
