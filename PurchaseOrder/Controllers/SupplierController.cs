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
        private readonly BaseDataAccess _dal = new BaseDataAccess();
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ILogger<SupplierController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name ="Search")]
        public async Task<IEnumerable<Supplier>> Get(string searchString)
        {
            var suppliers = new List<Supplier>();

        }
    }
}
