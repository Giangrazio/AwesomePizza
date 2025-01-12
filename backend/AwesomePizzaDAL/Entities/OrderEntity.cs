using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUnitOfWork.Entities;

namespace AwesomePizzaDAL.Entities
{
    [Table("Order", Schema = "Master")]
    public class OrderEntity : BaseAuditEntity
    {
        public DateTime OrderDate { get; set; }

        //public long? UserId { get; set; }
        //[ForeignKey(nameof(UserId))]
        //public UserEntity? User { get; set; }

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string DeliveryAddress { get; set; }

        // Current status of the order
        public OrderStatus Status { get; set; }

        // Unique code for tracking the order
        public string UniqueCode { get; set; }


        // Navigation property for related OrderProducts
        public ICollection<OrderProductEntity> OrderProducts { get; set; }
    }

    public enum OrderStatus
    {
        [Display(Name = "Pending")]
        Pending,       // The order has been created but not yet taken by the chef
        [Display(Name = "In preparation")]
        InPreparation, // The chef is currently preparing the order
        [Display(Name = "Completed")]
        Completed,     // The order preparation is complete
        [Display(Name = "Delivered")]
        Delivered,     // The order has been delivered to the customer
        [Display(Name = "Cancelled")]
        Cancelled      // The order has been cancelled
    }
}
