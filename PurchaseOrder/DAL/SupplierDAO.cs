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
        public Guid CreateSupplier(Supplier supplier)
        {

            
            var nameParam = this.GetParameter(Resources.NameParameterName, supplier.Name);
            var supplierCodeParam = this.GetParameter(Resources.SupplierCodeParameterName, supplier.SuppierCode);

            var tDr = GetDataReader("GetSupplierByProductCode", new List<DbParameter> { supplierCodeParam });
            if (tDr.HasRows)
                throw new Exception(Resources.SupplierAlreadyExistsError);

            var dr = this.GetDataReader(Resources.CreateSupplierProc, new List<DbParameter> { nameParam, supplierCodeParam });
            if (!dr.HasRows)
                throw new Exception("Failed to create supplier");

            var rs = dr.GetGuid(0);
            return rs;
        }

        public async Task<Supplier> GetSupplierByID(Guid supplierId)
        {
            var rs = new Supplier();
            var sIdParam = GetParameter(Resources.IdParameter, supplierId);
            var reader = GetDataReader(Resources.GetSupplierByIdProc, new List<DbParameter> { sIdParam });

            await reader.ReadAsync();
            rs.Id = reader.GetGuid(0);
            rs.SuppierCode = reader.GetString(1);
            rs.Name = reader.GetString(2);

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
                        SuppierCode = reader.GetString(1),
                        Name = reader.GetString(2)
                    };

                    res.Add(r);
                }

            await reader.CloseAsync();
            return res;
        }
    }
}
