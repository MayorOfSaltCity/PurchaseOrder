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
    public class PurchaseOrderController : ControllerBase
    {
        private readonly PurchaseOrderDAO _dal = new PurchaseOrderDAO();
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(ILogger<PurchaseOrderController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreatePurchaseOrder")]
        public async Task<Guid> CreatePurchaseOrder(Guid supplierId)
        {
            _logger.LogInformation($"Creating a new purchase order from supplier: {supplierId}");
            var id = await _dal.CreatePurchaseOrder(supplierId);
            return id;
        }

        [HttpGet(Name = "GetPurchaseOrder")]
        public async Task<PurchaseOrderModel> GetPurchaseOrder(Guid purchaseOrderId)
        {
            _logger.LogInformation("Fetching purchase order [{0}]", purchaseOrderId);
            var po = await _dal.GetPurchaseOrderById(purchaseOrderId);
            return po;
        }

        [HttpGet(Name = "GetSupplierOrders")]
        public async Task<IEnumerable<PurchaseOrderListDTO>> GetSupplierOrders(Guid supplierId)
        {
            _logger.LogInformation("Fetching purchase orders for supplier [{0}]", supplierId);
            var pos = await _dal.ListPurchaseOrders(supplierId);
            return pos;
        }

        [HttpPost (Name = "AddItem")]
        public async Task<Guid> AddProductToPurchaseOrder(Guid purchaseOrderId, Guid productId, int Quantity)
        {
            _logger.LogInformation("Adding [{0}] Item [{1}]] to Purchase Order [{2}]", Quantity, productId, purchaseOrderId);
            var poiId = await _dal.AddProductToPurchaseOrder(purchaseOrderId, productId, Quantity);
            return poiId;
        }        
    }
}
