using System;

namespace PurchaseOrder.Models
{
    public class Supplier : BaseModel<Guid>
    {
        public string SupplierCode { get; set; }
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
