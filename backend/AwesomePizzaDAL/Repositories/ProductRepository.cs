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
    public class ProductRepository : GenericRepository<ProductEntity>
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }
    }
}
