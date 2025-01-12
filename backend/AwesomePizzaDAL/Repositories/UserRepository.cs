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
    public class UserRepository : GenericRepository<UserEntity>
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}
