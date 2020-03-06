using PurchaseOrder.Models;
using PurchaseOrder.Properties;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrder.DAL
{
    public class SupplierDAO : BaseDataAccess
    {
        public SupplierDAO() : base(Resources.ConnectionString)
        {

        }

        /// <summary>
        /// Creates a new supplier in the database and returns the Guid related to that supplier
        /// </summary>
        /// <param name="supplier">The supplier data to populate to the database</param>
        /// <returns></returns>
        public async Task<Guid> CreateSupplier(Supplier supplier)
        {

            
            var nameParam = this.GetParameter(Resources.NameParameterName, supplier.Name);
            var supplierCodeParam = this.GetParameter(Resources.SupplierCodeParameterName, supplier.SupplierCode);

            var tDr = GetDataReader(Resources.GetSupplierByCodeProc, new List<DbParameter> { supplierCodeParam });
            if (tDr.HasRows)
                throw new Exception(string.Format(Resources.SupplierAlreadyExistsError, supplier.SupplierCode));
            supplierCodeParam = null;
            supplierCodeParam = this.GetParameter(Resources.SupplierCodeParameterName, supplier.SupplierCode);
            var dr = this.GetDataReader(Resources.CreateSupplierProc, new List<DbParameter> { nameParam, supplierCodeParam });
            if (!dr.HasRows)
                throw new Exception(Resources.CreateSupplierError);

            await dr.ReadAsync();
            var rs = dr.GetGuid(0);
            await dr.CloseAsync();
            return rs;
        }

        /// <summary>
        /// Gets the supplier by the Supplier Guid
        /// </summary>
        /// <param name="supplierId">The guid to get the supplier by</param>
        /// <returns>A Supplier Data Object</returns>
        public async Task<Supplier> GetSupplierByID(Guid supplierId)
        {
            var rs = new Supplier();
            var sIdParam = GetParameter(Resources.IdParameter, supplierId);
            var reader = GetDataReader(Resources.GetSupplierByIdProc, new List<DbParameter> { sIdParam });
            if (!reader.HasRows)
                throw new Exception(Resources.SupplierDoesNotExistErrorMessage);

            await reader.ReadAsync();
            rs.Id = reader.GetGuid(0);
            rs.SupplierCode = reader.GetString(1);
            rs.Name = reader.GetString(2);

            return rs;
        }

        /// <summary>
        /// Gets a supplier by the Supplier Code
        /// </summary>
        /// <param name="supplierCode">The supplier code</param>
        /// <returns>A supplier code object</returns>
        public async Task<Supplier> GetSupplierByCode(string supplierCode)
        {
            var rs = new Supplier();
            var sIdParam = GetParameter(Resources.SupplierCodeParameterName, supplierCode);
            var reader = GetDataReader(Resources.GetSupplierByCodeProc, new List<DbParameter> { sIdParam });
            if (!reader.HasRows)
                throw new Exception(Resources.SupplierDoesNotExistErrorMessage);

            await reader.ReadAsync();
            rs.Id = reader.GetGuid(0);
            rs.SupplierCode = reader.GetString(1);
            rs.Name = reader.GetString(2);
            await reader.CloseAsync();
            return rs;
        }

        public async Task<List<Supplier>> SearchSuppliers(string searchString)
        {
            var res = new List<Supplier>();
            var sParam = GetParameter(Resources.SupplierSearchParameterName, searchString);
            var reader = GetDataReader(Resources.SupplierSearchProcName, new List<DbParameter> { sParam });
            if (reader.HasRows)
                while (await reader.ReadAsync())
                {
                    var r = new Supplier
                    {
                        Id = reader.GetGuid(0),
                        SupplierCode = reader.GetString(1).Trim(),
                        Name = reader.GetString(2).Trim()
                    };

                    res.Add(r);
                }

            await reader.CloseAsync();
            return res;
        }
    }
}
