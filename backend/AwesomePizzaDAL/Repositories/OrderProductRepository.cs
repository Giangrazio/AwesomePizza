using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericUnitOfWork.Repository;
using Microsoft.EntityFrameworkCore;
using AwesomePizzaDAL.Entities;

namespace AwesomePizzaDAL.Repositories
{
    public class OrderProductRepository : GenericRepository<OrderProductEntity>
    {
        public OrderProductRepository(DbContext context) : base(context)
        {
        }
    }
}
