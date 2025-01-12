using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using GenericUnitOfWork.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GenericUnitOfWork.Repository
{
    public class DapperGenericRepository<T, TDto> : IDapperGenericRepository<T, TDto> where T : BaseEntity
    {
        //private readonly IConfiguration _config;
        //private readonly string _connectionString = "FVConnection";
        private readonly DbContext _context;
        private readonly IDbConnection _dbConnection;
        protected readonly Type entityType;

        public DapperGenericRepository(DbContext context)
        {
            _context = context;
            _dbConnection = context.Database.GetDbConnection();
            entityType = typeof(T);
        }
        public void Dispose()
        {

        }

        public int DapperExecute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }
        
        public int DapperCount(string where = null)
        {
            string table = AttributeReader.GetTableName<T>(_context);
            var sp = $"SELECT COUNT(*) FROM {table} {where}";
            return _dbConnection.Query<int>(sp, null).FirstOrDefault();
        }

        public T DapperGetById(long id, DynamicParameters parms = null)
        {
            string table = AttributeReader.GetTableName<T>(_context);
            var sp = $"SELECT * FROM {table} WHERE {table}.Id = {id}";
            return DapperGet(sp, parms, CommandType.Text);
        }
        
        public TDto DapperGetByIdToDto(long id, DynamicParameters parms = null)
        {
            string table = AttributeReader.GetTableName<T>(_context);
            var sp = $"SELECT * FROM {table} WHERE {table}.Id = {id}";
            return DapperGetDto(sp, parms, CommandType.Text);
        }

        public List<T> DapperGetAllById(List<long> id, DynamicParameters parms = null)
        {
            string table = AttributeReader.GetTableName<T>(_context);
            var sp = $"SELECT * FROM {table} WHERE {table}.Id IN ({string.Join(",", id)})";
            return DapperGetAll(sp, parms, CommandType.Text);
        }

        public List<TDto> DapperGetAllByIdToDto(List<long> id, DynamicParameters parms = null)
        {
            string table = AttributeReader.GetTableName<T>(_context);
            var sp = $"SELECT * FROM {table} WHERE {table}.Id IN ({string.Join(",",id)})";
            return DapperGetAllDto(sp, parms, CommandType.Text);
        }
        public T DapperGet(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            return _dbConnection.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public TDto DapperGetDto(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            return _dbConnection.Query<TDto>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public List<T> DapperGetAll(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            //using IDbConnection db = new SqlConnection(_config.GetConnectionString(_connectionString));
            return _dbConnection.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public List<TDto> DapperGetAllDto(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            //using IDbConnection db = new SqlConnection(_config.GetConnectionString(_connectionString));
            return _dbConnection.Query<TDto>(sp, parms, commandType: commandType).ToList();
        }

        public List<T> DapperGetAll(string sp, DynamicParameters parms)
        {
            //using IDbConnection db = new SqlConnection(_config.GetConnectionString(_connectionString));
            return _dbConnection.Query<T>(sp, parms).ToList();
            //return _dbConnection.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public T DapperAdd(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
           //using IDbConnection db = new SqlConnection(_config.GetConnectionString(_connectionString));
            try
            {
                if (_dbConnection.State == ConnectionState.Closed)
                    _dbConnection.Open();

                using var tran = _dbConnection.BeginTransaction();
                try
                {
                    result = _dbConnection.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
            }

            return result;
        }

        public T DapperUpdate(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            //using IDbConnection db = new SqlConnection(_config.GetConnectionString(_connectionString));
            try
            {
                if (_dbConnection.State == ConnectionState.Closed)
                    _dbConnection.Open();

                using var tran = _dbConnection.BeginTransaction();
                try
                {
                    result = _dbConnection.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
            return result;
        }
    }
}
