using AwesomePizzaDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomePizza.Tests.DatabaseSnapshot
{
    public static class OrderSnapshotExtension
    {

        public static DatabaseSnapshoter CreateOrder(this DatabaseSnapshoter snapshoter, out OrderEntity order, OrderEntity newOrder)
        {
            newOrder.OrderDate = DateTime.Now;
            newOrder.Status = OrderStatus.Pending;
            newOrder.UniqueCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            snapshoter.Database.Orders.Add(newOrder);
            snapshoter.Database.SaveChanges();
            order = newOrder;
            return snapshoter;
        }

        public static DatabaseSnapshoter GetAllOrders(this DatabaseSnapshoter snapshoter, out List<OrderEntity> orders)
        {
            orders = snapshoter.Database.Orders.ToList();
            return snapshoter;
        }

        public static DatabaseSnapshoter UpdateOrderStatus(this DatabaseSnapshoter snapshoter, out OrderStatus status, long orderId, OrderStatus newStatus)
        {
            var order = snapshoter.Database.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) { 
                throw new Exception("Order not found");
            }

            order.Status = newStatus;
            snapshoter.Database.SaveChanges();
            status = order.Status;

            return snapshoter;
        }
    }
}
