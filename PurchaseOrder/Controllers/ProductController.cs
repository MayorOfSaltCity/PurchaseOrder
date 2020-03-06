using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PurchaseOrder.DAL;
using PurchaseOrder.Models;
using PurchaseOrder.Properties;

namespace PurchaseOrder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductDAO _dal = new ProductDAO();
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet (Name = "SearchProducts")]
        public async Task<IEnumerable<Product>> SearchProducts(string searchString, bool isDeleted)
        {
            _logger.LogInformation(Resources.SearchProductsLogMessage, searchString);
            var products = await _dal.SearchProducts(searchString, isDeleted);
            return products;
        }

        [HttpPost (Name = "AddProductToSupplier")]
        public async Task<Guid> AddProductToSupplier(Product product)
        {
            _logger.LogInformation(Resources.AddProductToSupplierLogMessage, product.ProductCode, product.Supplier.Id);
            Guid newId = await _dal.AddProductToSupplier(product);
            return newId;
        }
    }
}
