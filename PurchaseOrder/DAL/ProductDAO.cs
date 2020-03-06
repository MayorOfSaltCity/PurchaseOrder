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

        public  async Task<List<Product>> SearchProducts(string searchString)
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
                        Price = reader.GetDecimal(4)
                    }
                }

            return products;
        }
    }
}
