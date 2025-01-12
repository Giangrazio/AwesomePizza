using GenericUnitOfWork.Repository;
using Microsoft.EntityFrameworkCore;
using AwesomePizzaDAL.Entities;

namespace AwesomePizzaDAL.Repositories
{
    public class OrderRepository : GenericRepository<OrderEntity>
    {
        public OrderRepository(DbContext context) : base(context)
        {
            
        }

        public new void Add(OrderEntity order)
        {
            // Generate a unique code for the order
            order.UniqueCode = GenerateUniqueCode();

            // set default status
            order.Status = OrderStatus.Pending;

            // set order date
            order.OrderDate = DateTime.Now;

            // Add the order to the database
            base.Add(order);
        }

        private string GenerateUniqueCode()
        {
            // Example implementation
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }
    }
}
