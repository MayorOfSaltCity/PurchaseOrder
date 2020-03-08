using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public async Task<IEnumerable<Supplier>> Search(string searchString)
        {
            _logger.LogInformation(Resources.SearchingSupplierLogMessage, searchString);
            try
            {
                var suppliers = await _dal.SearchSuppliers(searchString);

                return suppliers;
            } catch (Exception ex)
            {
                _logger.LogError("Error occurred searching suppliers {0}\t{1}", searchString, ex.Message);
                throw new HttpRequestException("Failed to add the supplier: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("/Supplier/Add")]
        public async Task<Supplier> AddSupplier(string name, string supplierCode)
        {
            _logger.LogInformation(Resources.CreatingSupplierLogMessage, supplierCode);
            try
            {
                var res = await _dal.CreateSupplier(name, supplierCode);
                var dSupplier = await _dal.GetSupplierByID(res);

                return dSupplier;
            } 
            catch (Exception ex)
            {
                _logger.LogError("Error occurred adding a supplier {0}\t{1}", supplierCode, ex.Message);
                throw new HttpRequestException("Failed to add the supplier: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("/Supplier/Fetch")]
        public async Task<Supplier> GetSupplier(string supplierCode)
        {
            _logger.LogInformation(Resources.GetSupplierByCodeProc);
            try
            {
                var supplier = await _dal.GetSupplierByCode(supplierCode);
                return supplier;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred fetching supplier {0}\t{1}", supplierCode, ex.Message);
                throw new HttpRequestException("Failed to fetch the supplier: " + ex.Message);
            }
        }
    }
}
