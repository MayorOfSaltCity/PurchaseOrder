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

            var suppliers = supplierControl.Search("Test").Result as List<Supplier>;
            Assert.IsTrue(suppliers.Count > 0, "No Test Suppliers please run the create test suppliers test");

            var supplier = suppliers.FirstOrDefault();
            Assert.IsNotNull(supplier, "NULL Value in list of suppliers");
            Assert.IsTrue(!string.IsNullOrEmpty(supplier.Name), "Supplier has no name");
            Assert.IsTrue(!string.IsNullOrEmpty(supplier.SupplierCode), "Supplier has no supplier code");

            var product = new Product
            {
                Description = "Test product",
                Price = 59.99,
                ProductCode = "TEST-PRODUCT-CODE",
                Supplier = supplier
            };

            Guid productId = productController.AddProductToSupplier(product).Result;

        }
    }
}
