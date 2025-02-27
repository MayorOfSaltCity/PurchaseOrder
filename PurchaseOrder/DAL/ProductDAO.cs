﻿using PurchaseOrder.Models;
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
                        Price = reader.GetDecimal(3)
                    };

                    products.Add(product);
                }

            await reader.CloseAsync();
            return products;
        }

        public async Task<Product> GetProductByProductId(Guid productId)
        {
            var products = new List<Product>();
            var idParam = GetParameter(Resources.ProductIdParameterName, productId);
            var reader = GetDataReader(Resources.GetProductByIdProc, new List<DbParameter> { idParam });

            if (!reader.HasRows)
                throw new Exception(Resources.ProductNotFound);
            await reader.ReadAsync();

            var product = new Product
            {
                Id = reader.GetGuid(0),
                ProductCode = reader.GetString(1),
                Description = reader.GetString(2),
                Price = reader.GetDecimal(3),
                Supplier = new Supplier
                {
                    Name = reader.GetString(4),
                    SupplierCode = reader.GetString(5),
                    Id = reader.GetGuid(6),
                    CreatedDate = reader.GetDateTime(7)
                }
            };



            await reader.CloseAsync();
            return product;
        }

        public async Task<List<Product>> GetProductsBySupplierId(Guid supplierId)
        {
            var products = new List<Product>();
            var idParam = GetParameter(Resources.SupplierIdParameterName, supplierId);
            var reader = GetDataReader(Resources.GetProductsBySupplierId, new List<DbParameter> { idParam });

            if (!reader.HasRows)
                throw new Exception(Resources.ProductNotFound);
            while (await reader.ReadAsync())
            {
                var product = new Product
                {
                    Id = reader.GetGuid(0),
                    ProductCode = reader.GetString(1),
                    Description = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    IsDeleted = reader.GetBoolean(8),
                    Supplier = new Supplier
                    {
                        Name = reader.GetString(4),
                        SupplierCode = reader.GetString(5),
                        Id = reader.GetGuid(6),
                        CreatedDate = reader.GetDateTime(7)
                    }
                };
                
                if (!(await reader.IsDBNullAsync(9)))
                    product.DeletedDate = reader.GetDateTime(9);

                products.Add(product);
            }


            await reader.CloseAsync();
            return products;
        }


        internal async Task<Guid> AddProductToSupplier(AddProductToSupplierModel product)
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
            var supplierIdParameter = GetParameter(Resources.SupplierIdParameterName, product.SupplierId);
            var priceParameter = GetParameter(Resources.ProductPriceParameterName, product.Price);
            var reader = GetDataReader(Resources.AddProductToSupplierProc, new List<DbParameter> { productCodeParameter, descriptionParameter, priceParameter, supplierIdParameter });

            if (reader.HasRows)
                await reader.ReadAsync();

            var resultantId = reader.GetGuid(0);
            await reader.CloseAsync();
            return resultantId;
        }

        internal async Task<bool> DeleteProduct(Guid productId)
        {
            var productIdParam = GetParameter(Resources.ProductIdParameterName, productId);
            var result = await ExecuteNonQueryAsync(Resources.DeleteProductByIdProc, new List<DbParameter> { productIdParam });
            return result == 0;
        }

        internal async Task<Guid> UpdateProduct(UpdateProductModel product)
        {
            var tIdParam = GetParameter(Resources.ProductIdParameterName, product.ProductId);
            var productReader = GetDataReader(Resources.GetProductByIdProc, new List<DbParameter> { tIdParam });

            if (!productReader.HasRows)
            {
                await productReader.CloseAsync();
                throw new Exception(string.Format(Resources.ProductDoesNotExistsErrorMessage, product.ProductId));
            }

            await productReader.CloseAsync();
            var idParam = GetParameter(Resources.IdParameter, product.ProductId);
            var descriptionParameter = GetParameter(Resources.ProductDescriptionParameterName, product.Description);
            var priceParameter = GetParameter(Resources.ProductPriceParameterName, product.Price);
            var reader = GetDataReader(Resources.UpdateProductProc, new List<DbParameter> { idParam, descriptionParameter, priceParameter });

            if (reader.HasRows)
                await reader.ReadAsync();

            var resultantId = reader.GetGuid(0);
            await reader.CloseAsync();
            return resultantId;
        }
    }
}
