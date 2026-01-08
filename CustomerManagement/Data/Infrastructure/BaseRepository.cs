using System;
using System.Collections.Generic;
using System.Data;
using CustomerManagement.Data.Infrastructure;
using MySql.Data.MySqlClient;

namespace CustomerManagement.Data.Infrastructure;

public abstract class BaseRepository
{
    protected readonly IUnitOfWork UnitOfWork;

    protected BaseRepository(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    protected IDbConnection Connection => UnitOfWork.Connection;
    protected IDbTransaction? Transaction => UnitOfWork.Transaction;

    protected IDbCommand CreateCommand(string sql, object? parameters = null)
    {
        var command = Connection.CreateCommand();
        command.Transaction = Transaction;
        command.CommandText = sql;

        if (parameters != null)
        {
            foreach (var prop in parameters.GetType().GetProperties())
            {
                var parameter = new MySqlParameter($"@{prop.Name}", prop.GetValue(parameters) ?? DBNull.Value);
                command.Parameters.Add(parameter);
            }
        }

        return command;
    }

    protected void ExecuteNonQuery(string sql, object? parameters = null)
    {
        try
        {
            using var command = CreateCommand(sql, parameters);
            command.ExecuteNonQuery();
        }
        catch (MySqlException ex)
        {
            throw new DataAccessException("Error executing database operation", ex);
        }
    }

    protected T ExecuteScalar<T>(string sql, object? parameters = null)
    {
        try
        {
            using var command = CreateCommand(sql, parameters);
            var result = command.ExecuteScalar();
            return (T)Convert.ChangeType(result, typeof(T));
        }
        catch (MySqlException ex)
        {
            throw new DataAccessException("Error executing database scalar operation", ex);
        }
    }

    protected IEnumerable<T> ExecuteReader<T>(string sql, Func<IDataReader, T> map, object? parameters = null)
    {
        var results = new List<T>();
        try
        {
            using var command = CreateCommand(sql, parameters);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                results.Add(map(reader));
            }
        }
        catch (MySqlException ex)
        {
            throw new DataAccessException("Error reading data from database", ex);
        }
        return results;
    }

    protected T? ExecuteSingle<T>(string sql, Func<IDataReader, T> map, object? parameters = null)
    {
        try
        {
            using var command = CreateCommand(sql, parameters);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return map(reader);
            }
            return default;
        }
        catch (MySqlException ex)
        {
            throw new DataAccessException("Error reading single record from database", ex);
        }
    }
}

public class DataAccessException : Exception
{
    public DataAccessException(string message, Exception innerException) : base(message, innerException) { }
}
