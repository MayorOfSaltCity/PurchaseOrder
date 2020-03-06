using System;

namespace PurchaseOrder.Models
{
    public class Product : BaseModel<Guid>
    {
        public string ProductCode { get; set; } 
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Supplier Supplier { get; set; }
    }
}
