using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models.Requests
{
    public class TestAddRequest
    {
        [Required]
        public string Data { get; set; } = null!;
    }
}
