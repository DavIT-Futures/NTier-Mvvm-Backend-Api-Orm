using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TestApp.Entity;

namespace TestApp.Server.SQL
{
    public static class SQLCommon<T> where T : EntityObject
    {
        public static string GetConnectionString()
        {
            return @"Data Source=ANDY_VAIO\SQLEXPRESS;Initial Catalog=TestApp;User ID=admin;Password=admin";
            //return @"Data Source=DAVIT-FUTURES\MSSQLSERVER2012;Initial Catalog=TestApp;User ID=super;Password=rfid.2k9";
            //return @"Data Source=DDOMOGAL\SQLEXPRESS2008R2;Initial Catalog=TestApp;Integrated Security=True;Pooling=False";
        }

        public static SqlDataReader ExecuteReader(string query, List<SqlParameter> sqlParams, SqlConnection connection, SqlTransaction transaction = null)
        {
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection, transaction);
            if (sqlParams != null)
                command.Parameters.AddRange(sqlParams.ToArray());
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        public static T ConvertRowToEntity(SqlDataReader reader)
        {
            T entity = Activator.CreateInstance<T>();

            //if property is marked as DataMember
            foreach (var item in typeof(T).GetProperties())
            {
                foreach (CustomAttributeData cad in item.CustomAttributes)
                {
                    if (cad.AttributeType == typeof(DataMemberAttribute))
                        item.SetValue(entity, reader[item.Name]);
                }
            }
            return entity;
        }

        public static string CreateWhereClause(T template, bool and, List<SqlParameter> sqlParams)
        {
            string result = string.Empty;
            List<string> whereClauses = new List<string>();

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                foreach (CustomAttributeData cad in prop.CustomAttributes)
                {
                    if (cad.AttributeType == typeof(DataMemberAttribute))
                    {
                        object propValue = prop.GetValue(template);
                        string propName = cad.NamedArguments[0].TypedValue.Value as string;
                        object defaultValue = null;
                        if (prop.PropertyType.IsValueType)
                            defaultValue = Activator.CreateInstance(prop.PropertyType);
                        if (propValue != null && propValue.Equals(defaultValue) == false)
                        {
                            whereClauses.Add(string.Format("{0} = @{1}", propName, propName));
                            sqlParams.Add(new SqlParameter("@" + propName, propValue));
                        }
                    }
                }
            }

            if (whereClauses.Count > 0)
            {
                result += " where ";
                for (int i = 0; i < whereClauses.Count; i++)
                {
                    result += whereClauses[i];
                    //if there is one more condition
                    if ((i + 1) < whereClauses.Count)
                        result += and ? "and" : "or"; //unig and or or depending on a flag
                }
            }
            return result;
        }
    }
}
