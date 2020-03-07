using PurchaseOrder.Models;
using PurchaseOrder.Properties;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrder.DAL
{
    public class PurchaseOrderDAO : BaseDataAccess
    {
        public PurchaseOrderDAO() : base(Resources.ConnectionString)
        {

        }

        public async Task<List<PurchaseOrderListDTO>> ListPurchaseOrders(Guid supplierId)
        {
            var supplierIdParam = GetParameter(Resources.SupplierIdParameterName, supplierId);
            var poReader = GetDataReader(Resources.GetPurchaseOrdersBySupplierProc, new List<DbParameter> { supplierIdParam });
            if (!poReader.HasRows)
                throw new Exception(string.Format(Resources.NoPurchaseOrdersForSupplierErrorMessage, supplierId));

            var purchaseOrders = new List<PurchaseOrderListDTO>();
            while (await poReader.ReadAsync())
            {
                var purchaseOrderLI = new PurchaseOrderListDTO
                {
                    Id = poReader.GetGuid(0),
                    Number = poReader.GetString(1),
                    CreatedDate = poReader.GetDateTime(2)
                };

                purchaseOrders.Add(purchaseOrderLI);
            }

            await poReader.CloseAsync();
            return purchaseOrders;
        }

        public async Task<Guid> CreatePurchaseOrder(Guid supplierId)
        {
            var supplierIdParam = GetParameter(Resources.SupplierIdParameterName, supplierId);
            var poCreateReader = GetDataReader(Resources.CreatePurchaseOrderProc, new List<DbParameter> { supplierIdParam });
            if (!poCreateReader.HasRows)
            {
                await poCreateReader.CloseAsync();
                throw new Exception(string.Format(Resources.CreatePurchaseOrderErrorMessage, supplierId));
            }

            await poCreateReader.ReadAsync();
            var guid = poCreateReader.GetGuid(0);

            await poCreateReader.CloseAsync();
            return guid;
        }

        public async Task<PurchaseOrderModel> GetPurchaseOrderById(Guid purchaseOrderID)
        {
            // get PO Record
            var idParam = GetParameter(Resources.PurchaseOrderIdParamName, purchaseOrderID);
            var reader = GetDataReader(Resources.GetPurchaseOrderProc, new List<DbParameter> { idParam });

            if (!reader.HasRows)
                throw new Exception(Resources.ProductNotFound);
            await reader.ReadAsync();


            var purchaseOrder = new PurchaseOrderModel
            {
                Id = reader.GetGuid(0),
                Number = reader.GetInt32(4),
                OrderDate = reader.GetDateTime(5),
                IsFinalized = reader.GetBoolean(6),
                Supplier = new Supplier
                {
                    Name = reader.GetString(3),
                    SupplierCode = reader.GetString(2),
                    Id = reader.GetGuid(1),
                    CreatedDate = reader.GetDateTime(8)
                }
            };

            if (purchaseOrder.IsFinalized)
                purchaseOrder.FinalizedDate = reader.GetDateTime(7);

            await reader.CloseAsync();

            var poiParam = GetParameter(Resources.POIdParameterName, purchaseOrderID);
            var poiReader = GetDataReader(Resources.GetPurchaseOrderItemsProc, new List<DbParameter> { poiParam });

            if (!poiReader.HasRows)
            {
                await poiReader.CloseAsync();
                return purchaseOrder;
            }

            await poiReader.CloseAsync();
            while (await poiReader.ReadAsync())
            {
                var purchaseOrderItem = new PurchaseOrderProduct
                {
                    Id = poiReader.GetGuid(0),
                    ProductCode = poiReader.GetString(1),
                    Price = poiReader.GetDecimal(2),
                    Description = poiReader.GetString(3),
                    Quantity = poiReader.GetInt32(4)
                };

                purchaseOrder.Products.Add(purchaseOrderItem);
            }

            await poiReader.CloseAsync();
            return purchaseOrder;
        }


        internal async Task<Guid> AddProductToPurchaseOrder(Guid purchaseOrderId, Guid productId, int quantity)
        {
            var productIdParam = GetParameter(Resources.ProductIdParameterName, productId);
            var purchaseOrderIdParam = GetParameter(Resources.PurchaseOrderIdParamName, purchaseOrderId);
            var quantityParam = GetParameter(Resources.PurchaseOrderQuantityParameterName, quantity);
            var addItemProc = GetDataReader(Resources.AddProductToPurchaseOrderProc, new List<DbParameter> { purchaseOrderIdParam, productIdParam, quantityParam });
            if (!addItemProc.HasRows)
            {
                await addItemProc.CloseAsync();
                throw new Exception(string.Format(Resources.AddItemToPurchaseOrderFailedErrorMessage, purchaseOrderId, productId));
            }
            await addItemProc.ReadAsync();
            var itemId = addItemProc.GetGuid(0);

            await addItemProc.CloseAsync();
            return itemId;
        }

        internal async Task<bool> RemoveProductFromPurchaseOrder(Guid purchaseOrderId, Guid productId)
        {
            var purchaseOrderParam = GetParameter(Resources.PurchaseOrderIdParamName, purchaseOrderId);
            var poReader = GetDataReader(Resources.IsPurchaseOrderFinalizedProc, new List<DbParameter> { GetParameter(Resources.PurchaseOrderIdParamName, purchaseOrderId) });
            if (!poReader.HasRows)
            {
                await poReader.CloseAsync();
                throw new Exception(string.Format(Resources.PurchaseOrderNotFoundErrorMessage, purchaseOrderId));
            }
            await poReader.ReadAsync();
            var isFinalized = poReader.GetBoolean(0);
            if (isFinalized)
            {
                await poReader.CloseAsync();
                throw new Exception(string.Format(Resources.PurchaseOrderIsFinalizedErrorMessage, purchaseOrderId));
            }

            await poReader.CloseAsync();
            var productIdParam = GetParameter(Resources.ProductIdParameterName, productId);
            var result = await ExecuteNonQueryAsync(Resources.DeleteProductFromPOByIdProc, new List<DbParameter> { productIdParam, purchaseOrderParam });
            return result == 0;
        }

        internal async Task<bool> FinalizePurchaseOrder(Guid  purchaseOrderId)
        {
            var poIdParam = GetParameter(Resources.PurchaseOrderIdParamName, purchaseOrderId);
            var fin = await ExecuteNonQueryAsync(Resources.FinalizePurchaseOrderProc, new List<DbParameter> { poIdParam });

            return fin == -1;
        }
    }
}
