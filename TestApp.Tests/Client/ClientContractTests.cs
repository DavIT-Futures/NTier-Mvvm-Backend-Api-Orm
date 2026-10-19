using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestApp.Client.EntityWCFService;
using TestApp.Entity;

namespace TestApp.Tests.Client
{
    [TestClass]
    public class ClientContractTests
    {
        [TestMethod]
        public void ClientProxyInterface_CanBeMockedWithMoq()
        {
            var mockClient = new Mock<IEntityWCFService>();

            mockClient
                .Setup(c => c.GetById(typeof(ProductEntity).FullName!, 100))
                .Returns(new ProductEntity { ProductId = 100, Name = "Client Product", Description = "Sample description" });

            var result = mockClient.Object.GetById(typeof(ProductEntity).FullName!, 100) as ProductEntity;

            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.ProductId);
            Assert.AreEqual("Client Product", result.Name);
            mockClient.Verify(c => c.GetById(typeof(ProductEntity).FullName!, 100), Times.Once);
        }

        [TestMethod]
        public void ClientProxyInterface_SimulatesCommunicationException()
        {
            var mockClient = new Mock<IEntityWCFService>();

            mockClient
                .Setup(c => c.GetAll(It.IsAny<string>()))
                .Throws(new CommunicationException("Endpoint not reachable"));

            var ex = Assert.Throws<CommunicationException>(() =>
            {
                mockClient.Object.GetAll(typeof(ProductEntity).FullName!);
            });

            StringAssert.Contains(ex.Message, "Endpoint not reachable");
        }

        [TestMethod]
        public void ClientChannelInterface_CanBeMockedWithMoq()
        {
            var mockChannel = new Mock<IEntityWCFServiceChannel>();
            Assert.IsNotNull(mockChannel);
            Assert.IsTrue(mockChannel.Object is IEntityWCFService);
            Assert.IsTrue(mockChannel.Object is IClientChannel);
        }

        [TestMethod]
        public void ClientProxyInterface_MockGetByTemplate_ReturnsFilteredResults()
        {
            var mockClient = new Mock<IEntityWCFService>();
            var expectedEmployees = new EntityObject[]
            {
                new EmployeeEntity { EmployeeId = 3, FirstName = "Charlie", LastName = "Brown" }
            };

            mockClient
                .Setup(c => c.GetByTemplate(typeof(EmployeeEntity).FullName!, It.IsAny<EntityObject>()))
                .Returns(expectedEmployees);

            var result = mockClient.Object.GetByTemplate(typeof(EmployeeEntity).FullName!, new EmployeeEntity { LastName = "Brown" });

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("Charlie", ((EmployeeEntity)result[0]).FirstName);
        }
    }
}
