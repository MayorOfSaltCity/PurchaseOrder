using Microsoft.VisualStudio.TestTools.UnitTesting;
using PurchaseOrder.Controllers;
using PurchaseOrder.Models;
using System.Collections.Generic;
using System.Linq;

namespace PurchaseOrderUnitTests
{
    [TestClass]
    public class SupplierUnitTests
    {
        [TestMethod]
        public void GetSupplierBySupplierCodeTest()
        {
            var controller = new SupplierController(new LoggerStub<SupplierController>());
            var suppliers = controller.Search("Test").Result as List<Supplier>;
            var code = suppliers.FirstOrDefault()?.SupplierCode;
            var supplier = controller.GetSupplier(code);
            Assert.IsNotNull(supplier, "No supplier returned");
        }

        [TestMethod]
        public void SearchSupplierTest()
        {
            var controller = new SupplierController(new LoggerStub<SupplierController>());
            var searchString = "Test";
            var suppliers = controller.Search(searchString).Result as List<Supplier>;
            if (suppliers.Count == 0)
            {
                Assert.Inconclusive("there are no Test suppliers in the database, please run the CreateSupplierTest Test Case");
            } else
            {
                foreach (var supplier in suppliers)
                {
                    Assert.IsTrue(!string.IsNullOrEmpty(supplier.Name));
                    Assert.IsTrue(!string.IsNullOrEmpty(supplier.SupplierCode));
                }
            }
        }

        [TestMethod]
        public void CreateSupplierTest()
        {
            var controller = new SupplierController(new LoggerStub<SupplierController>());
            List<Supplier> suppliers = controller.Search("Test Supplier").Result as List<Supplier>;
            var no = string.Format("{0:000}", suppliers.Count);
            var supplier = new Supplier
            {
                Name = "Test Supplier",
                SupplierCode = $"TEST-SUPPLIER-CODE-{no}"
            };

            var supplierId = controller.AddSupplier(supplier.Name, supplier.SupplierCode).Result;
            Assert.IsNotNull(supplierId, "Failed to create supplier");
            var dataSupplier = controller.GetSupplier(supplier.SupplierCode).Result;
            Assert.IsNotNull(dataSupplier, "Could not read supplier from Database");
            Assert.AreEqual(supplier.Name, dataSupplier.Name, "Supplier Name Corrupted on Save");
            Assert.AreEqual(supplier.SupplierCode, dataSupplier.SupplierCode, "Supplier Code Corrupted on Save");
        }
    }
}
