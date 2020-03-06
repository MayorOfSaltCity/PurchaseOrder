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

        public async Task<List<Product>> SearchProducts(string searchString, bool isDeleted)
        {
            var products = new List<Product>();
            var searchParam = GetParameter(Resources.ProductSeachParameterName, searchString);
            var isDeletedParam = GetParameter(Resources.IsDeletedParameterName, isDeleted);
            var reader = GetDataReader(Resources.SearchProductsProc, new List<DbParameter> { searchParam, isDeletedParam });

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
            var testCodeParam = GetParameter(Resources.ProductCodeParameterName, product.ProductCode);
            var fetchAllParam = GetParameter(Resources.FetchAllParameterName, true);
            var productReader = GetDataReader(Resources.GetProductByProductCodeProc, new List<DbParameter> { testCodeParam, fetchAllParam });

            if (productReader.HasRows)
            {
                await productReader.CloseAsync();
                throw new Exception(string.Format(Resources.ProductExistsErrorMessage, product.ProductCode)); 
            }

            await productReader.CloseAsync();
            var descriptionParameter = GetParameter(Resources.ProductDescriptionParameterName, product.Description);
            var productCodeParameter = GetParameter(Resources.ProductCodeParameterName, product.ProductCode);
            var supplierIdParameter = GetParameter(Resources.SupplierIdParameterName, product.Supplier.Id);
            var priceParameter = GetParameter(Resources.ProductPriceParameterName, product.Price);
            var reader = GetDataReader(Resources.AddProductToSupplierProc, new List<DbParameter> { productCodeParameter, descriptionParameter, priceParameter, supplierIdParameter });

            if (reader.HasRows)
                await reader.ReadAsync();

            var resultantId = reader.GetGuid(0);
            await reader.CloseAsync();
            return resultantId;
        }

        internal async Task<Guid> UpdateProduct(Product product)
        {
            var testCodeParam = GetParameter(Resources.ProductCodeParameterName, product.ProductCode);
            var fetchAllParam = GetParameter(Resources.FetchAllParameterName, true);
            var productReader = GetDataReader(Resources.GetProductByProductCodeProc, new List<DbParameter> { testCodeParam });

            if (!productReader.HasRows)
            {
                await productReader.CloseAsync();
                throw new Exception(string.Format(Resources.ProductDoesNotExistsErrorMessage, product.ProductCode));
            }

            await productReader.CloseAsync();
            var descriptionParameter = GetParameter(Resources.ProductDescriptionParameterName, product.Description);
            var productCodeParameter = GetParameter(Resources.ProductCodeParameterName, product.ProductCode);
            var supplierIdParameter = GetParameter(Resources.SupplierIdParameterName, product.Supplier.Id);
            var priceParameter = GetParameter(Resources.ProductPriceParameterName, product.Price);
            var reader = GetDataReader(Resources.UpdateProductProc, new List<DbParameter> { productCodeParameter, descriptionParameter, priceParameter, supplierIdParameter });

            if (reader.HasRows)
                await reader.ReadAsync();

            var resultantId = reader.GetGuid(0);
            await reader.CloseAsync();
            return resultantId;
        }
    }
}
