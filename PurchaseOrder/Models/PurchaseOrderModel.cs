using System;
using System.Collections.Generic;

namespace PurchaseOrder.Models
{
    public class PurchaseOrderModel : BaseModel<Guid>
    {
        public Supplier Supplier { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int Number { get; set; }
        public HashSet<PurchaseOrderProduct> Products { get; set; } = new HashSet<PurchaseOrderProduct>();

        public bool IsFinalized { get; set; }

        public DateTime? FinalizedDate { get; set; }

    }
    public class PurchaseOrderProduct : Product
    {
        public int Quantity { get; set; }
    }
    public class PurchaseOrderListDTO
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
