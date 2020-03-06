using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PurchaseOrder.DAL;
using PurchaseOrder.Models;


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

        [HttpGet(Name ="Search")]
        public async Task<IEnumerable<Supplier>> Search(string searchString)
        {
            var suppliers = await _dal.SearchSuppliers(searchString);
            
            return suppliers;

        }

        [HttpPost(Name = "AddSupplier")]
        public async Task<Supplier> AddSupplier(Supplier supplier)
        {
            var rSupplier = new Supplier();
            return rSupplier;
        }
    }
}
