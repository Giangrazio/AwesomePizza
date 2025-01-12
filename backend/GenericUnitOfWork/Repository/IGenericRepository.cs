using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EFCore.BulkExtensions;

namespace GenericUnitOfWork.Repository
{
    public interface IGenericRepository<T> : IDisposable
    {
        void Add(T entity);
        void AddRange(List<T> entities);
        void UpdateRange(List<T> entities, bool notUpdateLastTokenId = false);

        void Update(T entity, bool notUpdateLastTokenId = false);

        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        T? GetById(long id);

        void Delete(T entity);
        void Delete(object id);
        void DeleteRange(IEnumerable<T> entities);

        IQueryable<T> Table { get; }
        List<T> GetAll();

        void DeleteAll(bool resetIdentity = false);
        void TruncateTable();
        void DeleteCollectionByIds(List<int> ids);

        int ExecuteSqlRaw(string sql, params object[] list);

        void BulkInsert(List<T> entities, BulkConfig bulkConfig = null);

        void BulkUpdate(List<T> entities, BulkConfig bulkConfig = null);

        void BulkDelete(List<T> entities, BulkConfig bulkConfig = null);

        void BulkInsertOrUpdate(List<T> entities, BulkConfig bulkConfig = null);

        void BulkInsertOrUpdateOrDelete(List<T> entities, BulkConfig bulkConfig = null);

        void BulkRead(List<T> entities, BulkConfig bulkConfig = null);

        void BulkTruncate();
    }
}
