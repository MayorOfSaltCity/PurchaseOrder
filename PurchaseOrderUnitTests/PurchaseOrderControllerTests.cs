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
            Assert.Fail();
        }

        [TestMethod]
        public void TestRemoveItemFromPurchaseOrder()
        {
            Assert.Fail();
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
