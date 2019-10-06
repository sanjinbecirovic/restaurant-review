using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.DataAccess.Repositories.Interfaces;

namespace RestaurantReview.DataAccess.Repositories
{
    public class ReplyRepository : GenericRepository<Reply>, IReplyRepository
    {
        public ReplyRepository(RestaurantReviewContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<Reply> GetReplyForReviewAsync(int reviewId)
            => await this.dbContext.Replies.AsNoTracking().FirstOrDefaultAsync(r => r.ReviewId == reviewId);
    }
}
