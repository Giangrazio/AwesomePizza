using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace GenericUnitOfWork.Repository
{
    public interface IDapperGenericRepository<T, TDto> : IDisposable
    {
        T DapperGetById(long id, DynamicParameters parms = null);
        TDto DapperGetByIdToDto(long id, DynamicParameters parms = null);
        T DapperGet(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        TDto DapperGetDto(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        List<T> DapperGetAll(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<TDto> DapperGetAllDto(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int DapperExecute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T DapperAdd(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        T DapperUpdate(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        int DapperCount(string where);
    }
}
