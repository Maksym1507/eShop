namespace Infrastructure.RateLimit.Models
{
    public class RateLimitModel
    {
        public int QuantityOfRequests { get; set; }

        public DateTime TimeOfRequest { get; set; }
    }
}
