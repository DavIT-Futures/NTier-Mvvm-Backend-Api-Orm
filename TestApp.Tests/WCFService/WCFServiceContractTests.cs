using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestApp.Entity;
using TestApp.WCFService;

namespace TestApp.Tests.WCFService
{
    [TestClass]
    public class WCFServiceContractTests
    {
        [TestMethod]
        public void IEntityWCFService_HasServiceContractAttribute()
        {
            var serviceContract = typeof(IEntityWCFService).GetCustomAttribute<ServiceContractAttribute>();
            Assert.IsNotNull(serviceContract, "IEntityWCFService must be annotated with [ServiceContract]");
        }

        [TestMethod]
        public void IEntityWCFService_Methods_HaveOperationContractAttribute()
        {
            var methods = typeof(IEntityWCFService).GetMethods();
            Assert.IsTrue(methods.Length >= 3);

            foreach (var method in methods)
            {
                var opContract = method.GetCustomAttribute<OperationContractAttribute>();
                Assert.IsNotNull(opContract, $"Method {method.Name} must be annotated with [OperationContract]");
            }
        }

        [TestMethod]
        public void IEntityWCFService_MockGetById_ReturnsExpectedEntity()
        {
            var mockService = new Mock<IEntityWCFService>();
            var expectedProduct = new ProductEntity { ProductId = 7, Name = "Mocked Product" };

            mockService
                .Setup(s => s.GetById(typeof(ProductEntity).FullName!, 7))
                .Returns(expectedProduct);

            var result = mockService.Object.GetById(typeof(ProductEntity).FullName!, 7);

            Assert.IsNotNull(result);
            Assert.AreEqual(7, result.Id);
            Assert.AreEqual("Mocked Product", result.Name);
            mockService.Verify(s => s.GetById(typeof(ProductEntity).FullName!, 7), Times.Once);
        }

        [TestMethod]
        public void IEntityWCFService_MockGetAll_ReturnsPopulatedList()
        {
            var mockService = new Mock<IEntityWCFService>();
            var expectedList = new List<EntityObject>
            {
                new EmployeeEntity { EmployeeId = 1, FirstName = "Anna" },
                new EmployeeEntity { EmployeeId = 2, FirstName = "Ben" }
            };

            mockService
                .Setup(s => s.GetAll(typeof(EmployeeEntity).FullName!))
                .Returns(expectedList);

            var list = mockService.Object.GetAll(typeof(EmployeeEntity).FullName!);

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("Anna", ((EmployeeEntity)list[0]).FirstName);
            mockService.Verify(s => s.GetAll(typeof(EmployeeEntity).FullName!), Times.Once);
        }

        [TestMethod]
        public void IEntityWCFService_MockGetByTemplate_MatchesTemplateParameter()
        {
            var mockService = new Mock<IEntityWCFService>();
            var template = new ProductEntity { ProductType = "Gadgets" };
            var expectedList = new List<EntityObject>
            {
                new ProductEntity { ProductId = 10, ProductType = "Gadgets", Name = "Smartwatch" }
            };

            mockService
                .Setup(s => s.GetByTemplate(typeof(ProductEntity).FullName!, It.Is<EntityObject>(t => ((ProductEntity)t).ProductType == "Gadgets")))
                .Returns(expectedList);

            var result = mockService.Object.GetByTemplate(typeof(ProductEntity).FullName!, template);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Smartwatch", result[0].Name);
            mockService.Verify(s => s.GetByTemplate(typeof(ProductEntity).FullName!, It.IsAny<EntityObject>()), Times.Once);
        }

        [TestMethod]
        public void IEntityWCFService_MockService_SimulatesFaultException()
        {
            var mockService = new Mock<IEntityWCFService>();

            mockService
                .Setup(s => s.GetById(It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new FaultException("Entity not found in database"));

            var ex = Assert.Throws<FaultException>(() =>
            {
                mockService.Object.GetById("NonExistent", -999);
            });

            StringAssert.Contains(ex.Message, "Entity not found");
        }

        [TestMethod]
        public void EntityWCFService_ImplementsIEntityWCFService()
        {
            var service = new EntityWCFService();
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(IEntityWCFService));
        }
    }
}
