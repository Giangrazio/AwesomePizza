using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomePizzaBLL.Models
{
    public class OrderProductModel : BaseAuditModel
    {
        public long? OrderId { get; protected set; }
        public OrderModel? Order { get; protected set; }

        public long? ProductId { get; set; }
        public ProductModel? Product { get; protected set; }

        public int Quantity { get; set; }

        public string? Note { get; set; }
    }
}
