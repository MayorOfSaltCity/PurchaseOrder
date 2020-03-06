﻿using PurchaseOrder.Models;
using PurchaseOrder.Properties;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
    }
}
