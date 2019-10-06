namespace RestaurantReview.Web.Infrastructure
{
    public class JwtTokenSettings
    {
        public string SigningKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpireDays { get; set; }
    }
}
