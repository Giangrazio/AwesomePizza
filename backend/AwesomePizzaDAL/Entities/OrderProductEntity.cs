using System.ComponentModel.DataAnnotations.Schema;
using GenericUnitOfWork.Entities;

namespace AwesomePizzaDAL.Entities
{
    [Table("OrderProduct", Schema = "Master")]
    public class OrderProductEntity : BaseAuditEntity
    {
        public long OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public OrderEntity Order { get; set; }

        public long ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public ProductEntity Product { get; set; }

        public int Quantity { get; set; }

        public string? Note { get; set; }
    }
}
