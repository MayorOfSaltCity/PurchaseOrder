using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrder.Models
{
    public abstract class BaseModel<T>
    {
        public T Id { get; set; }
    }
    public class PurchaseOrderModel : BaseModel<Guid>
    {
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public HashSet<Product> Products { get; set; } = new HashSet<Product>();

        public bool IsFinalized { get; set; }

    }

    public class Product : BaseModel<Guid>
    {
        public string ProductCode { get; set; } 
        public string Description { get; set; }
        public Price Price { get; set; }
        public Supplier Supplier { get; set; }
    }

    public class Supplier : BaseModel<Guid>
    {
        public string SuppierCode { get; set; }
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
