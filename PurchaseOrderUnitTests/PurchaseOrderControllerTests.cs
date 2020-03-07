using Microsoft.VisualStudio.TestTools.UnitTesting;
using PurchaseOrder.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PurchaseOrder.Models;

namespace PurchaseOrderUnitTests
{
    [TestClass]
    public class PurchaseOrderControllerTests
    {
        [TestMethod]
        public void TestCreatePurchaseOrder()
        {
            var supplierController = new SupplierController(new LoggerStub<SupplierController>());
            var poController = new PurchaseOrderController(new LoggerStub<PurchaseOrderController>());

            var suppliers = supplierController.Search("Test").Result as List<Supplier>;
            Assert.IsNotNull(suppliers, "Suppliers are null");
            Assert.IsTrue(suppliers.Count > 0, "No test suppliers in database");
            var supplier = suppliers.FirstOrDefault();
            var poId = poController.CreatePurchaseOrder(supplier.Id).Result;
            Assert.IsNotNull(poId, "Purchase order controller returned NULL");
            Assert.AreNotEqual(poId, Guid.Empty, "Purchase Order ID is Empty GUID");
        }

        [TestMethod]
        public void TestAddItemToPurchaseOrder()
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
            var poController = new PurchaseOrderController(new LoggerStub<PurchaseOrderController>());

            var poId = poController.CreatePurchaseOrder(supplier.Id).Result;
            Assert.IsNotNull(poId, "Purchase order controller returned NULL");
            Assert.AreNotEqual(poId, Guid.Empty, "Purchase Order ID is Empty GUID");
            var poiId = poController.AddProductToPurchaseOrder(poId, productId, 5);
            Assert.IsNotNull(poiId, "Failed to add item to purchase order");
            Assert.AreNotEqual(poiId, Guid.Empty, "Purchase Order Item ID is Empty GUID");
        }

        [TestMethod]
        public void TestFinalizePurhcaseOrder()
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
            var poController = new PurchaseOrderController(new LoggerStub<PurchaseOrderController>());

            var poId = poController.CreatePurchaseOrder(supplier.Id).Result;
            Assert.IsNotNull(poId, "Purchase order controller returned NULL");
            Assert.AreNotEqual(poId, Guid.Empty, "Purchase Order ID is Empty GUID");
            var poiId = poController.AddProductToPurchaseOrder(poId, productId, 5);
            Assert.IsNotNull(poiId, "Failed to add item to purchase order");
            Assert.AreNotEqual(poiId, Guid.Empty, "Purchase Order Item ID is Empty GUID");
            // var finalized = poController.FinalizePurchaseOrder(poId);

            var finalized = poController.FinalizePurchaseOrder(poId);
        }

        [TestMethod]
        public void TestRemoveItemFromPurchaseOrder()
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
            var poController = new PurchaseOrderController(new LoggerStub<PurchaseOrderController>());

            var poId = poController.CreatePurchaseOrder(supplier.Id).Result;
            Assert.IsNotNull(poId, "Purchase order controller returned NULL");
            Assert.AreNotEqual(poId, Guid.Empty, "Purchase Order ID is Empty GUID");
            var poiId = poController.AddProductToPurchaseOrder(poId, productId, 5).Result;
            Assert.IsNotNull(poiId, "Failed to add item to purchase order");
            Assert.AreNotEqual(poiId, Guid.Empty, "Purchase Order Item ID is Empty GUID");

            var removed = poController.RemoveItemFromPurchaseOrder(poId, productId).Result;
            Assert.IsTrue(removed, "Item was not removed from the purchase order");
        }

        [TestMethod]
        public void TestListPurchaseOrders()
        {
            var supplierController = new SupplierController(new LoggerStub<SupplierController>());
            var poController = new PurchaseOrderController(new LoggerStub<PurchaseOrderController>());

            var suppliers = supplierController.Search("Test").Result as List<Supplier>;
            Assert.IsNotNull(suppliers, "Suppliers are null");
            Assert.IsTrue(suppliers.Count > 0, "No test suppliers in database");
            var supplier = suppliers.FirstOrDefault();
            var poList = poController.GetSupplierOrders(supplier.Id).Result as List<PurchaseOrderListDTO>;
            Assert.IsTrue(poList.Count > 0, "Test supplier has no purchase orders, please create test data");
        }

        [TestMethod]
        public void TestGetPurchaseOrder()
        {
            var supplierController = new SupplierController(new LoggerStub<SupplierController>());
            var poController = new PurchaseOrderController(new LoggerStub<PurchaseOrderController>());

            var suppliers = supplierController.Search("Test").Result as List<Supplier>;
            Assert.IsNotNull(suppliers, "Suppliers are null");
            Assert.IsTrue(suppliers.Count > 0, "No test suppliers in database");
            var supplier = suppliers.FirstOrDefault();
            var poList = poController.GetSupplierOrders(supplier.Id).Result as List<PurchaseOrderListDTO>;
            Assert.IsTrue(poList.Count > 0, "Test supplier has no purchase orders, please create test data");
            var po = poList.First();
            var purchaseOrder = poController.GetPurchaseOrder(po.Id).Result as PurchaseOrderModel;
            Assert.IsNotNull(purchaseOrder, "Purchase order is null");
            Assert.AreEqual(po.Id, purchaseOrder.Id, "Wrong purchase order returned");
        }
    }
}
