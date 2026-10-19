using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Entity;
using TestApp.Server.Service;
using TestApp.Server.SQL;

namespace TestApp.Tests.Server
{
    [TestClass]
    public class ServerServicesTests
    {
        [TestMethod]
        public void EntityService_ProductEntity_CanBeInstantiated()
        {
            var service = new EntityService<ProductEntity>();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void EntityService_EmployeeEntity_CanBeInstantiated()
        {
            var service = new EntityService<EmployeeEntity>();
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void ProductService_InheritsFromEntityService()
        {
            var productService = new ProductService();
            Assert.IsNotNull(productService);
            Assert.IsInstanceOfType(productService, typeof(EntityService<ProductEntity>));
        }

        [TestMethod]
        public void EmployeeService_InheritsFromEntityService()
        {
            var employeeService = new EmployeeService();
            Assert.IsNotNull(employeeService);
            Assert.IsInstanceOfType(employeeService, typeof(EntityService<EmployeeEntity>));
        }

        [TestMethod]
        public void SQLEntityDataAccess_ProductEntity_InitializesCorrectly()
        {
            var dataAccess = new SQLEntityDataAccess<ProductEntity>();
            Assert.IsNotNull(dataAccess);
        }

        [TestMethod]
        public void SQLEntityDataAccess_EmployeeEntity_InitializesCorrectly()
        {
            var dataAccess = new SQLEntityDataAccess<EmployeeEntity>();
            Assert.IsNotNull(dataAccess);
        }

        [TestMethod]
        public void SQLProductDataAccess_InheritsFromSQLEntityDataAccess()
        {
            var type = typeof(ProductService).Assembly.GetType("TestApp.Server.SQL.SQLProductDataAccess");
            Assert.IsNotNull(type);
            var dataAccess = Activator.CreateInstance(type!);
            Assert.IsNotNull(dataAccess);
            Assert.IsInstanceOfType(dataAccess, typeof(SQLEntityDataAccess<ProductEntity>));
        }

        [TestMethod]
        public void SQLEmployeeDataAccess_InheritsFromSQLEntityDataAccess()
        {
            var type = typeof(EmployeeService).Assembly.GetType("TestApp.Server.SQL.SQLEmployeeDataAccess");
            Assert.IsNotNull(type);
            var dataAccess = Activator.CreateInstance(type!);
            Assert.IsNotNull(dataAccess);
            Assert.IsInstanceOfType(dataAccess, typeof(SQLEntityDataAccess<EmployeeEntity>));
        }
    }
}
