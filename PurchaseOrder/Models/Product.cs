using System;

namespace PurchaseOrder.Models
{
    public class Product : BaseModel<Guid>
    {
        public string ProductCode { get; set; } 
        public string Description { get; set; }
        public double Price { get; set; }
        public Supplier Supplier { get; set; }
    }
}
