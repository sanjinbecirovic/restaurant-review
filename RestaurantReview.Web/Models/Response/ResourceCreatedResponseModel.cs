namespace RestaurantReview.Web.Models.Response
{
    public class ResourceCreatedResponseModel<TKey>
    {
        public TKey Id { get; set; }

        public ResourceCreatedResponseModel(TKey id)
        {
            this.Id = id;
        }
    }
}
