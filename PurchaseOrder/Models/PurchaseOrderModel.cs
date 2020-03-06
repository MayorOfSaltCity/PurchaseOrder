using System;
using System.Collections.Generic;

namespace PurchaseOrder.Models
{
    public class PurchaseOrderModel : BaseModel<Guid>
    {
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public HashSet<Product> Products { get; set; } = new HashSet<Product>();

        public bool IsFinalized { get; set; }

    }
}
