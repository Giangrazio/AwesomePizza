using EFCore.BulkExtensions;
using GenericUnitOfWork.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;

namespace GenericUnitOfWork.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        public HttpContext CurrentHttpContext { get; internal set; }

        public IQueryable<T> Table => throw new NotImplementedException();

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if(entity is BaseAuditEntity)
            {
                var baseAuditEntity = entity as BaseAuditEntity;
                baseAuditEntity.CreatedOnDate = DateTime.Now;
                if(CurrentHttpContext != null && CurrentHttpContext.Request != null)
                {
                    baseAuditEntity.CreatedByIpAddress = CurrentHttpContext.Request.Host.Value;
                    if (CurrentHttpContext.User.Identity != null && CurrentHttpContext.User != null && CurrentHttpContext.User.Identity.IsAuthenticated)
                    {
                        baseAuditEntity.CreatedByUserId = CurrentHttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
                        baseAuditEntity.MembershipEmail = CurrentHttpContext.User.Identity.Name;
                        baseAuditEntity.MembershipName = CurrentHttpContext.User.Identity.Name;
                        baseAuditEntity.LastTokenId = CurrentHttpContext.User.Claims.FirstOrDefault(claim => claim.Type == nameof(BaseAuditEntity.LastTokenId))?.Value;
                    }
                }
            }
            //entity.CreatedOnDate = DateTime.Now;
            _context.Set<T>().Add(entity);
        }

        public void AddRange(List<T> entities)
        {
            if (entities is List<BaseAuditEntity>)
            {
                var createdOnDate = DateTime.Now; // Giusto per essere sicuri che su salvataggi "impegnativi" la data sia identica anche su ms
                string createdByIpAddress = null;
                string createdByUserId = null;
                string membershipEmail = null;
                string membershipName = null;
                string tokenId = null;
                if (CurrentHttpContext != null && CurrentHttpContext.Request != null)
                {
                    createdByIpAddress = CurrentHttpContext.Request.Host.Value;
                    if (CurrentHttpContext.User.Identity != null && CurrentHttpContext.User != null && CurrentHttpContext.User.Identity.IsAuthenticated)
                    {
                        createdByUserId = CurrentHttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
                        membershipEmail = CurrentHttpContext.User.Identity.Name;
                        membershipName = CurrentHttpContext.User.Identity.Name;
                        tokenId = CurrentHttpContext.User.Claims.FirstOrDefault(claim => claim.Type == nameof(BaseAuditEntity.LastTokenId))?.Value;
                    }
                }
                entities.ForEach(e =>
                {
                    var baseAuditEntity = e as BaseAuditEntity;
                    baseAuditEntity.CreatedOnDate = createdOnDate;
                    baseAuditEntity.CreatedByIpAddress = createdByIpAddress;
                    baseAuditEntity.CreatedByUserId = createdByUserId;
                    baseAuditEntity.MembershipEmail = membershipEmail;
                    baseAuditEntity.MembershipName = membershipName;
                    baseAuditEntity.LastTokenId = tokenId;
                });
            }
            _context.Set<T>().AddRange(entities);
        }

        public void UpdateRange(List<T> entities, bool notUpdateLastTokenId = false)
        {
            if (entities is List<BaseAuditEntity>)
            {
                var modifiedDate = DateTime.Now;
                string tokenId = null;
                if (CurrentHttpContext != null && CurrentHttpContext.Request != null)
                {
                    if (CurrentHttpContext.User.Identity != null && CurrentHttpContext.User != null && CurrentHttpContext.User.Identity.IsAuthenticated)
                    {
                        tokenId = CurrentHttpContext.User.Claims.FirstOrDefault(claim => claim.Type == nameof(BaseAuditEntity.LastTokenId))?.Value;
                    }
                }
                entities.ForEach(e =>
                {
                    if (e is BaseAuditEntity baseAuditEntity)
                    {
                        baseAuditEntity.ModifiedDate = modifiedDate;
                        if (!notUpdateLastTokenId)
                        {
                            baseAuditEntity.LastTokenId = tokenId;
                        }
                    }
                });
            }
            _context.Set<T>().UpdateRange(entities);
        }

        public void Update(T entity, bool notUpdateLastTokenId = false)
        {
            //entity.ModifiedDate = DateTime.Now;
            if (entity.Id <= 0)
            {
                throw new Exception("Impossibile effettuare l'update, id non pervenuto");
            }
            if (entity is BaseAuditEntity)
            {
                var baseAuditEntity = entity as BaseAuditEntity;
                baseAuditEntity.ModifiedDate = DateTime.Now;
                if (!notUpdateLastTokenId && CurrentHttpContext != null && CurrentHttpContext.Request != null)
                {
                    if (CurrentHttpContext.User.Identity != null && CurrentHttpContext.User != null && CurrentHttpContext.User.Identity.IsAuthenticated)
                    {
                        baseAuditEntity.LastTokenId = CurrentHttpContext.User.Claims.FirstOrDefault(claim => claim.Type == nameof(BaseAuditEntity.LastTokenId))?.Value;
                    }
                }
            }
            _context.Set<T>().Update(entity);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public T? GetById(long id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Delete(object id)
        {
            T entityToDelete = _context.Set<T>().Find(id);
            Delete(entityToDelete);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public List<T> GetAll()
        {
            return GetQueryable(null, o => o.OrderBy(n => n.Id)).ToList();
        }

        public void TruncateTable()
        {
            var tblName = AttributeReader.GetTableName<T>(_context);
            string query = $"TRUNCATE TABLE {tblName}";
            ExecuteSqlRaw(query);
        }

        public void DeleteAll(bool resetIdentity = false)
        {
            var tblName = AttributeReader.GetTableName<T>(_context);
            string query = $"DELETE FROM {tblName}";
            if (resetIdentity)
            {
                string reset = $"DBCC CHECKIDENT ('{tblName}', RESEED, 0);";
                reset += Environment.NewLine;
                reset += "GO";
                query += reset + Environment.NewLine + query;
            }

            ExecuteSqlRaw(query);
        }

        public void DeleteCollectionByIds(List<int> ids)
        {
            if (ids.Any())
            {
                //DELETE FROM table WHERE id IN (?,?,?,?,?,?,?,?)
                var collection = string.Join(",", ids);

                var tblName = AttributeReader.GetTableName<T>(_context);
                string query = $"DELETE FROM {tblName} WHERE Id IN ( {collection} )";

                ExecuteSqlRaw(query);
            }

        }

        public int ExecuteSqlRaw(string sql, params object[] list)
        {
            string[] splitcommands = sql.Split(new string[] { "GO\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> commandList = new List<string>(splitcommands);

            int sumExec = 0;
            foreach (var command in commandList)
            {
                sumExec += _context.Database.ExecuteSqlRaw(command, list.ToArray());
            }

            return sumExec;
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #region Bulk Method
        public void BulkInsert(List<T> entities, BulkConfig bulkConfig = null)
        {
            _context.BulkInsert(entities, bulkConfig);
        }

        public void BulkUpdate(List<T> entities, BulkConfig bulkConfig = null)
        {
            _context.BulkUpdate(entities, bulkConfig);
        }

        public void BulkDelete(List<T> entities, BulkConfig bulkConfig = null)
        {
            _context.BulkDelete(entities, bulkConfig);
        }

        public void BulkInsertOrUpdate(List<T> entities, BulkConfig bulkConfig = null)
        {
            _context.BulkInsertOrUpdate(entities, bulkConfig);
        }

        public void BulkInsertOrUpdateOrDelete(List<T> entities, BulkConfig bulkConfig = null)
        {
            _context.BulkInsertOrUpdateOrDelete(entities, bulkConfig);
        }

        public void BulkRead(List<T> entities, BulkConfig bulkConfig = null)
        {
            _context.BulkRead(entities, bulkConfig);
        }

        public void BulkTruncate()
        {
            _context.Truncate<T>();
        }

        #endregion
    }

    public static class AttributeReader
    {
        //Get DB Table Name
        public static string GetTableName<T>(DbContext context) where T : class
        {
            // We need dbcontext to access the models
            var models = context.Model;

            // Get all the entity types information
            var entityTypes = models.GetEntityTypes();

            // T is Name of class
            var entityTypeOfT = entityTypes.First(t => t.ClrType == typeof(T));

            var tableNameAnnotation = entityTypeOfT.GetAnnotation("Relational:TableName");
            var schemaNameAnnotation = entityTypeOfT.GetAnnotation("Relational:Schema");
            var TableName = tableNameAnnotation.Value.ToString();
            if (!string.IsNullOrEmpty(schemaNameAnnotation.Value!.ToString()))
            {
                return $"[{schemaNameAnnotation.Value.ToString()}].[{TableName}]";
            }
            return $"[{ TableName}]";
        }

    }
}
