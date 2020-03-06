using Microsoft.VisualStudio.TestTools.UnitTesting;
using PurchaseOrder.Controllers;
using PurchaseOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PurchaseOrderUnitTests
{
    [TestClass]
    public class ProductControllerTests
    {
        [TestMethod]
        public void AddProductToSupplierTest()
        {
            var productController = new ProductController(new LoggerStub<ProductController>());
            var supplierControl = new SupplierController(new LoggerStub<SupplierController>());
            var products = productController.SearchProducts(string.Empty, true).Result as List<Product>;
            var c = string.Format("{0:000}", products.Count);
            var suppliers = supplierControl.Search("Test").Result as List<Supplier>;
            Assert.IsTrue(suppliers.Count > 0, "No Test Suppliers please run the create test suppliers test");

            var supplier = suppliers.FirstOrDefault();
            Assert.IsNotNull(supplier, "NULL Value in list of suppliers");
            Assert.IsTrue(!string.IsNullOrEmpty(supplier.Name), "Supplier has no name");
            Assert.IsTrue(!string.IsNullOrEmpty(supplier.SupplierCode), "Supplier has no supplier code");

            var product = new Product
            {
                Description = "Test product",
                Price = 59.99M,
                ProductCode = $"TEST-PRODUCT-CODE-{c}",
                Supplier = supplier
            };

            Guid productId = productController.AddProductToSupplier(product).Result;

        }
        
        [TestMethod]
        public void SearchProductTest()
        {
            var productController = new ProductController(new LoggerStub<ProductController>());
            var products = productController.SearchProducts("Test", false).Result as List<Product>;
            Assert.IsTrue(products.Count > 0);

            var product = products.FirstOrDefault();
            Assert.IsTrue(!string.IsNullOrEmpty(product.Description), "Product has null description");
            Assert.IsTrue(!string.IsNullOrEmpty(product.ProductCode), "Product has null code");
            Assert.IsTrue(product.Price > 0, "Product has bad price");
        }

        [TestMethod]
        public void DeleteProductTest()
        {
            var productController = new ProductController(new LoggerStub<ProductController>());
            var products = productController.SearchProducts("Test", false).Result as List<Product>;
            Assert.IsTrue(products.Count > 0);

            var product = products.FirstOrDefault();
            var deleted = productController.DeleteProduct(product.Id).Result;
            Assert.IsTrue(deleted, "Product failed to delete");
        }

        [TestMethod]
        public void UpdateProductTest()
        {
            var productController = new ProductController(new LoggerStub<ProductController>());
            var products = productController.SearchProducts("Test", false).Result as List<Product>;
            Assert.IsTrue(products.Count > 0);

            var product = products.FirstOrDefault();
            
            var newPrice = product.Price * 1.2M;
            product.Price = newPrice;
            var newId = productController.UpdateProduct(product).Result;
            var updatedProduct = productController.GetProduct(newId).Result;
            Assert.AreEqual(updatedProduct.Price, newPrice, "Product failed to update");
        }

        [TestMethod]
        public void GetProductByProductId()
        {
            var productController = new ProductController(new LoggerStub<ProductController>());
            var products = productController.SearchProducts("Test", false).Result as List<Product>;
            Assert.IsTrue(products.Count > 0);

            var product = products.FirstOrDefault();
            Assert.IsNull(product.Supplier);
            var dbProduct = productController.GetProduct(product.Id).Result;
            Assert.IsNotNull(dbProduct, "Product failed to fetch");
            Assert.IsNotNull(dbProduct.Supplier, "Product Supplier failed to load");
        }
    }
}
