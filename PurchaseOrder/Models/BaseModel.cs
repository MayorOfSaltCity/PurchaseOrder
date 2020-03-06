using System.Linq;
using System.Threading.Tasks;

namespace PurchaseOrder.Models
{
    public abstract class BaseModel<T>
    {
        public T Id { get; set; }
    }
}
