using System;
using System.Threading;
using System.Threading.Tasks;
using GenericUnitOfWork.Entities;
using GenericUnitOfWork.Repository;
using Microsoft.EntityFrameworkCore;

namespace GenericUnitOfWork.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> RepositoryFor<T>() where T : BaseEntity;
        T GenericRepository<T>();
        
        //TRepository GenericRepository<TEntity, TRepository>()
        //    where TEntity : BaseEntity
        //    where TRepository : GenericRepository<TEntity>;
        //IDapperGenericRepository<T, TDto> Dapper<T,TDto>();

        T GenericDapperRepository<T>();
        void SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbContext GetContext();
    }
}
