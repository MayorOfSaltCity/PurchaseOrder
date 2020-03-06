using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PurchaseOrder.Models;


namespace PurchaseOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet (Name = "SearchProducts")]
        public IEnumerable<Product> Get(string searchString)
        {
            var rng = new Random();
            return null;
        }
    }
}
