using System;
using System.Collections.Generic;
using System.Text;
using Dapper.Contrib.Extensions;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System.Collections.Concurrent;
using System.Reflection;

namespace InsuranceBackend.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected string _connectionString;
        public Repository(string connectionString)
        {
            SqlMapperExtensions.TableNameMapper = (type) => { return $"{ type.Name}"; };
            _connectionString = connectionString;
        }

        public bool Delete(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Delete(entity);
            }
        }

        public T GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Get<T>(id);
            }
        }

        public IEnumerable<T> GetList()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<T>();
            }
        }

        public int Insert(T entity)
        {
            DapperHack();
            using (var connection = new SqlConnection(_connectionString))
            {
                return (int)connection.Insert(entity);
            }
        }

        public bool Update(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Update(entity);
            }
        }

        private static void DapperHack()
        {
            var cache = typeof(SqlMapperExtensions).GetField("KeyProperties", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null)
                as ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>;

            cache?.Clear();
        }
    }
}

