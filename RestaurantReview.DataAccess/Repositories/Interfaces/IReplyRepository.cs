using System.Threading.Tasks;
using RestaurantReview.DataAccess.Entities;

namespace RestaurantReview.DataAccess.Repositories.Interfaces
{
    public interface IReplyRepository : IGenericRepository<Reply>
    {
        Task<Reply> GetReplyForReviewAsync(int reviewId);
    }
}
