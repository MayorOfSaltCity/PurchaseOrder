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

        [HttpGet]
        [Route("SearchProducts")]
        public async Task<IEnumerable<Product>> SearchProducts(string searchString, bool isDeleted)
        {
            _logger.LogInformation(Resources.SearchProductsLogMessage, searchString);
            var products = await _dal.SearchProducts(searchString, isDeleted);
            return products;
        }

        [HttpPost]
        [Route("AddProductToSupplier")]
        public async Task<Guid> AddProductToSupplier(Product product)
        {
            _logger.LogInformation(Resources.AddProductToSupplierLogMessage, product.ProductCode, product.Supplier.Id);
            Guid newId = await _dal.AddProductToSupplier(product);
            return newId;
        }

        [HttpPost]
        [Route("DeleteProduct")]
        public async Task<bool> DeleteProduct(Guid productId)
        {
            _logger.LogInformation(Resources.DeletingProductbyId, productId);
            bool deleted = await _dal.DeleteProduct(productId);
            return deleted;
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<Guid> UpdateProduct(Product product)
        {
            _logger.LogInformation(Resources.DeletingProductbyId, product.Id);
            Guid updateProduct = await _dal.UpdateProduct(product);
            return updateProduct;
        }

        [HttpGet]
        [Route("GetProduct")]
        public async Task<Product> GetProduct(Guid productId)
        {
            _logger.LogInformation(Resources.GetProductByProductIdLogMessage, productId);
            Product product = await _dal.GetProductByProductId(productId);
            return product;
        }

        [HttpGet]
        [Route("GetSupplierProducts")]
        public async Task<IEnumerable<Product>> GetSupplierProducts(Guid supplierId)
        {
            _logger.LogInformation(Resources.GetProductByProductIdLogMessage, supplierId);
            var products = await _dal.GetProductsBySupplierId(supplierId);
            return products;
        }
    }
}
