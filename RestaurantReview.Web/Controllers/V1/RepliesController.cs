using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReview.DataAccess.Entities;
using RestaurantReview.DataAccess.Repositories.Interfaces;
using RestaurantReview.Web.Extensions;
using RestaurantReview.Web.Models.Request;
using RestaurantReview.Web.Models.Response;

namespace RestaurantReview.Web.Controllers.V1
{
    [Route("api/v{version:apiVersion}/restaurant/{restaurantId:int}/reviews/{reviewId:int}/[controller]")]
    public class RepliesController : ApiControllerBase
    {
        private readonly IReplyRepository replyRepository;

        public RepliesController(IReplyRepository replyRepository, IMapper mapper)
            : base(mapper)
        {
            this.replyRepository = replyRepository;
        }

        // GET: api/restaurants/1/reviews/1/reply
        [HttpGet]
        [ProducesResponseType(typeof(ReplyResponseModel), 200)]
        public async Task<IActionResult> Get(int restaurantId, int reviewId)
        {
            var reply = await this.replyRepository.GetReplyForReviewAsync(reviewId);
            var response = Mapper.Map<ReplyResponseModel>(reply);

            return Ok(response);
        }

        // POST: api/restaurants/1/reviews/1/reply
        [HttpPost]
        [ProducesResponseType(typeof(ReplyResponseModel), 201)]
        public async Task<IActionResult> Post(int restaurantId, int reviewId, [FromBody] CreateReplyRequestModel model)
        {
            var reply = CreateReply(User.GetUserId(), model, reviewId, null);
            reply = await this.replyRepository.CreateAsync(reply);

            var response = Mapper.Map<ReplyResponseModel>(reply);

            return Created("", response);

        }

        // PUT: api/restaurants/1/reviews/1/reply/1
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ReplyResponseModel), 200)]
        public async Task<IActionResult> Put(int restaurantId, int reviewId, int id, [FromBody] CreateReplyRequestModel model)
        {
            var reply = CreateReply(User.GetUserId(), model, reviewId, id);
            reply = await this.replyRepository.UpdateAsync(reply);

            var response = Mapper.Map<ReplyResponseModel>(reply);
            return Ok(response);
        }

        // DELETE: api/restaurants/1/reviews/1/reply/1
        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(int restaurantId, int reviewId, int id)
        {
            await this.replyRepository.DeleteAsync(id);
            return Ok();
        }

        private Reply CreateReply(string userId, CreateReplyRequestModel model, int reviewId, int? replyId)
        {
            return new Reply
            {
                Id = replyId ?? 0,
                ReviewId = reviewId,
                UserId = userId,
                Text = model.Text,
                TimeStamp = DateTime.Now
            };
        }
    }
}
