using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestApp.Entity;

namespace TestApp.Server.SQL
{
    public class SQLEntityDataAccess<T> where T : EntityObject
    {
        int shortSleep;
        int longSleep;
        string tableName;
        string keyName;

        public SQLEntityDataAccess()
        {
            shortSleep = 200;
            longSleep = 400;
            T entity = Activator.CreateInstance<T>();
            tableName = entity.GetTableName();
            keyName = entity.GetKeyName();
        }

        public T GetById(int id)
        {
            Thread.Sleep(shortSleep);
            T result;

            using (SqlConnection connection = new SqlConnection(SQLCommon<T>.GetConnectionString()))
            {
                string query = string.Format("select * from {0} where {1}={2}", tableName, keyName, id);
                SqlDataReader reader = SQLCommon<T>.ExecuteReader(query, null, connection);
                reader.Read();
                result = SQLCommon<T>.ConvertRowToEntity(reader);
            }

            return result;
        }

        public List<T> GetByTemplate(T template, bool and = false)
        {
            Thread.Sleep(shortSleep);
            List<T> result = new List<T>();
            List<string> whereClauses = new List<string>();
            List<SqlParameter> sqlParams = new List<SqlParameter>();

            using (SqlConnection connection = new SqlConnection(SQLCommon<T>.GetConnectionString()))
            {
                string query = string.Format("select * from {0}", tableName);
                query += SQLCommon<T>.CreateWhereClause(template, and, sqlParams);

                SqlDataReader reader = SQLCommon<T>.ExecuteReader(query, sqlParams, connection);
                while (reader.Read())
                {
                    result.Add(SQLCommon<T>.ConvertRowToEntity(reader));
                }
            }

            return result;
        }

        public List<T> GetAll()
        {
            Thread.Sleep(longSleep);
            List<T> result = new List<T>();

            using (SqlConnection connection = new SqlConnection(SQLCommon<T>.GetConnectionString()))
            {
                string query = string.Format("select * from {0}", tableName);
                SqlDataReader reader = SQLCommon<T>.ExecuteReader(query, null, connection);
                while (reader.Read())
                {
                    result.Add(SQLCommon<T>.ConvertRowToEntity(reader));
                }
            }

            return result;
        }
    }
}
