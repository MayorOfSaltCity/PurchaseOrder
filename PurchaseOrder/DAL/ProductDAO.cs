using PurchaseOrder.Models;
using PurchaseOrder.Properties;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrder.DAL
{
    public class ProductDAO : BaseDataAccess
    {
        public ProductDAO() : base(Resources.ConnectionString)
        {

        }

        public async Task<List<Product>> SearchProducts(string searchString)
        {
            var products = new List<Product>();
            var searchParam = GetParameter(Resources.ProductSeachParameterName, searchString);
            var reader = GetDataReader(Resources.SearchProductsProc, new List<DbParameter> { searchParam });

            if (reader.HasRows)
                while (await reader.ReadAsync())
                {
                    var product = new Product
                    {
                        Id = reader.GetGuid(0),
                        ProductCode = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDouble(4)
                    };

                    products.Add(product);
                }

            await reader.CloseAsync();
            return products;
        }

        internal async Task<Guid> AddProductToSupplier(Product product)
        {
            var descriptionParameter = GetParameter(Resources.ProductDescriptionParameterName, product.Description);
            var productCodeParameter = GetParameter(Resources.ProductCodeParameterName, product.ProductCode);
            var supplierIdParameter = GetParameter(Resources.SupplierIdParameterName, product.Supplier.Id);
            var priceParameter = GetParameter(Resources.ProductPriceParameterName, product.Price);
            var reader = GetDataReader(Resources.SearchProductsProc, new List<DbParameter> { productCodeParameter, descriptionParameter, priceParameter, supplierIdParameter });

            if (reader.HasRows)
                await reader.ReadAsync();

            var resultantId = reader.GetGuid(0);
            await reader.CloseAsync();
            return resultantId;
        }
    }
}
