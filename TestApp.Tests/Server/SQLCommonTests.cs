using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Entity;
using TestApp.Server.SQL;

namespace TestApp.Tests.Server
{
    [TestClass]
    public class SQLCommonTests
    {
        [TestMethod]
        public void GetConnectionString_ReturnsNonEmptyString()
        {
            string connectionString = SQLCommon<ProductEntity>.GetConnectionString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(connectionString));
        }

        [TestMethod]
        public void CreateWhereClause_DefaultTemplate_ReturnsEmptyAndNoParameters()
        {
            var template = new ProductEntity();
            var sqlParams = new List<SqlParameter>();

            string where = SQLCommon<ProductEntity>.CreateWhereClause(template, true, sqlParams);

            Assert.AreEqual(string.Empty, where);
            Assert.AreEqual(0, sqlParams.Count);
        }

        [TestMethod]
        public void CreateWhereClause_SinglePropertySet_GeneratesCorrectWhereClauseAndParam()
        {
            var template = new ProductEntity
            {
                ProductType = "CategoryA"
            };
            var sqlParams = new List<SqlParameter>();

            string where = SQLCommon<ProductEntity>.CreateWhereClause(template, true, sqlParams);

            StringAssert.StartsWith(where, " where ");
            StringAssert.Contains(where, "ProductType = @ProductType");

            var param = sqlParams.Find(p => p.ParameterName == "@ProductType");
            Assert.IsNotNull(param);
            Assert.AreEqual("CategoryA", param.Value);
        }

        [TestMethod]
        public void CreateWhereClause_ValueTypeProperty_AddsParameterWhenNotDefault()
        {
            var template = new ProductEntity
            {
                Number = 555
            };
            var sqlParams = new List<SqlParameter>();

            string where = SQLCommon<ProductEntity>.CreateWhereClause(template, true, sqlParams);

            StringAssert.Contains(where, "Number = @Number");
            var param = sqlParams.Find(p => p.ParameterName == "@Number");
            Assert.IsNotNull(param);
            Assert.AreEqual(555, param.Value);
        }

        [TestMethod]
        public void CreateWhereClause_EmployeeTemplate_GeneratesExpectedClause()
        {
            var template = new EmployeeEntity
            {
                FirstName = "Bob",
                LastName = "Marley"
            };
            var sqlParams = new List<SqlParameter>();

            string where = SQLCommon<EmployeeEntity>.CreateWhereClause(template, true, sqlParams);

            StringAssert.StartsWith(where, " where ");
            StringAssert.Contains(where, "FirstName = @FirstName");
            StringAssert.Contains(where, "LastName = @LastName");

            Assert.IsTrue(sqlParams.Exists(p => p.ParameterName == "@FirstName" && (string)p.Value == "Bob"));
            Assert.IsTrue(sqlParams.Exists(p => p.ParameterName == "@LastName" && (string)p.Value == "Marley"));
        }

        [TestMethod]
        public void CreateWhereClause_MultipleConditions_HandlesAndOrFlag()
        {
            var templateAnd = new EmployeeEntity
            {
                FirstName = "Alice",
                LastName = "Cooper"
            };
            var sqlParamsAnd = new List<SqlParameter>();
            string whereAnd = SQLCommon<EmployeeEntity>.CreateWhereClause(templateAnd, true, sqlParamsAnd);
            StringAssert.Contains(whereAnd, "and");

            var templateOr = new EmployeeEntity
            {
                FirstName = "Alice",
                LastName = "Cooper"
            };
            var sqlParamsOr = new List<SqlParameter>();
            string whereOr = SQLCommon<EmployeeEntity>.CreateWhereClause(templateOr, false, sqlParamsOr);
            StringAssert.Contains(whereOr, "or");
        }
    }
}
