using AutoMapper;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.Web.Models.Response;

namespace RestaurantReview.Web.Models.Mapping_Profiles
{
    public class ReplyResponseModel_MappingProfile : Profile
    {
        public ReplyResponseModel_MappingProfile()
        {
            CreateMap<Reply, ReplyResponseModel>().ReverseMap();
        }
    }
}
