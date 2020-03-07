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
    public class SupplierController : ControllerBase
    {
        private readonly SupplierDAO _dal = new SupplierDAO();
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ILogger<SupplierController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/Supplier/Search")]
        public async Task<IEnumerable<Supplier>> Get(string searchString)
        {
            var suppliers = await _dal.SearchSuppliers(searchString);
            
            return suppliers;

        }

        [HttpPost]
        [Route("/Supplier/Add")]
        public async Task<Supplier> AddSupplier(Supplier supplier)
        {
            _logger.LogInformation(Resources.CreatingSupplierLogMessage, supplier.SupplierCode);
            var res = await _dal.CreateSupplier(supplier);
            var dSupplier = await _dal.GetSupplierByID(res);
            return dSupplier;
        }

        [HttpGet]
        [Route("/Supplier/Fetch")]
        public async Task<Supplier> GetSupplier(string supplierCode)
        {
            _logger.LogInformation(Resources.GetSupplierByCodeProc);
            var supplier = await _dal.GetSupplierByCode(supplierCode);
            return supplier;
        }
    }
}
