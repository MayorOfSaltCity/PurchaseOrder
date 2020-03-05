using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrder.Models
{
    public class PurchaseOrderModel
    {
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public HashSet<PurchaseOrderProduct> Products { get; set; } = new HashSet<PurchaseOrderProduct>();

    }

    public class Product
    {
        public string ProductCode { get; set; } 
        public string Description { get; set; }
        public Price Price { get; set; }
        public Supplier Supplier { get; set; }
    }

    public class Supplier
    {
        public string SuppierCode { get; set; }
        public string Name { get; set; }
    }

    public class PurchaseOrderProduct
    {
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public PurchaseOrderPrice Price { get; set; }
        public PurchaseOrderSupplier Supplier { get; set; }

    }

    public class PurchaseOrderSupplier 
    {

    }
}
