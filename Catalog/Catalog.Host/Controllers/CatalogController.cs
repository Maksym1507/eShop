using Catalog.Host.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private static readonly CatalogEntity[] _products =
        {
            new CatalogEntity
            {
                Id = 1,
                Title = "Pizza Saint Diablo",
                UrlImg = "https://roll-club.kh.ua/wp-content/uploads/2021/04/svjatoi-diablo-1.jpg.webp",
                Price = 245,
                Consist = "mozzarella, mushrooms, salami, pesto sauce, tomato sauce, oregano",
                Weight = 870
            },
            new CatalogEntity
            {
                Id = 2,
                Title = "Pepperoni",
                UrlImg = "https://roll-club.kh.ua/wp-content/uploads/2014/08/pepperoni.jpg.webp",
                Price = 235,
                Consist = "Pepperoni, hunting sausages, feta, olives, sriracha, mozzarella, oregano, tomato sauce",
                Weight = 800
            },
            new CatalogEntity
            {
                Id = 3,
                Title = "Pizza Boom of meat",
                UrlImg = "https://roll-club.kh.ua/wp-content/uploads/2014/08/mjasnoj-bum.jpg.webp",
                Price = 245,
                Consist = "mozzarella, hunting sausages, bacon, boiled pork, blue onion, tomato sauce, parsley, oregano",
                Weight = 850
            },
            new CatalogEntity
            {
                Id = 4,
                Title = "European",
                UrlImg = "https://roll-club.kh.ua/wp-content/uploads/2014/08/evropejskaja.jpg.webp",
                Price = 149,
                Consist = "mozzarella, chicken fillet, hunting sausages, champignons, parsley, ham, tomato sauce, oregano",
                Weight = 550
            },
            new CatalogEntity
            {
                Id = 5,
                Title = "Seafood",
                UrlImg = "https://roll-club.kh.ua/wp-content/uploads/2014/08/dary-morja.jpg.webp",
                Price = 229,
                Consist = "mozzarella, salmon, mussels, squid, tomato, lemon, pesto, oregano, tomato sauce",
                Weight = 550
            },
            new CatalogEntity
            {
                Id = 6,
                Title = "Pizza with ham and mushrooms",
                UrlImg = "https://roll-club.kh.ua/wp-content/uploads/2015/09/vechtchina-griby.jpg.webp",
                Price = 239,
                Consist = "mozzarella, ham, olives, tomatoes, parsley, mushrooms, oregano, tomato sauce",
                Weight = 900
            },
        };

        private readonly ILogger<CatalogController> _logger;

        public CatalogController(ILogger<CatalogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<CatalogEntity> Get()
        {
            return _products;
        }
    }
}