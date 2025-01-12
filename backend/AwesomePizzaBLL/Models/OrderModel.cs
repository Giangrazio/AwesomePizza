using AwesomePizzaBLL.Extensions;
using AwesomePizzaDAL.Entities;

namespace AwesomePizzaBLL.Models
{
    public class OrderModel : BaseAuditModel
    {
        public DateTime OrderDate { get; set; }

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string DeliveryAddress { get; set; }

        // Current status of the order
        public OrderStatus? Status { get; protected set; }
        public string? StatusValue => Status?.ToValueName();
        public string? StatusDisplay => Status?.GetDisplayName();

        // Unique code for tracking the order
        public string? UniqueCode { get; protected set; }

        // Navigation property for related OrderProducts
        public ICollection<OrderProductModel> OrderProducts { get; set; }
    }
}
