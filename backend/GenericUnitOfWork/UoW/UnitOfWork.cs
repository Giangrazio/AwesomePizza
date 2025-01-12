using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using GenericUnitOfWork.Entities;
using GenericUnitOfWork.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GenericUnitOfWork.UoW
{

    public partial class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly HttpContext ? _httpContext;

        public UnitOfWork(DbContext context, HttpContext? httpContext = null)
        {
            _context = context;
            _httpContext = httpContext;
        }

        //public TRepository GenericRepository<TRepository, TEntity>()
        //    where TEntity : BaseEntity
        //    where TRepository : GenericRepository<TEntity>
        //{
        //    TRepository result = (TRepository)Activator.CreateInstance(typeof(TRepository), _context);
        //    result.CurrentHttpContext = _httpContext;
        //    return result;
        //}

        public T GenericRepository<T>()
        {
            T result = (T)Activator.CreateInstance(typeof(T), _context)!;
            // Non è la soluzione ideale. Però è l'unica che non richiede modifiche al codice in tutti i microservizi
            // Se invece si è disposti a fare modifiche ovuque, la soluzione migliore è quella commentata sopra
            var propertyInfos = typeof(T).GetProperties().Where(p => p.PropertyType == typeof(HttpContext));
            foreach (var propertyInfo in propertyInfos)
            {
                propertyInfo.SetValue(result, _httpContext);
            }
            return result;
        }

        public T GenericDapperRepository<T>()
        {
            return (T)Activator.CreateInstance(typeof(T), _context)!;
        }

        /// <summary>
        /// Generic Repository for internal Database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IGenericRepository<T> RepositoryFor<T>() where T : BaseEntity
        {
            var result = new GenericRepository<T>(_context);
            if (_httpContext != null) result.CurrentHttpContext = _httpContext;
            return result;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public DbContext GetContext()
        {
            return _context;
        }

        //private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
    }
}
