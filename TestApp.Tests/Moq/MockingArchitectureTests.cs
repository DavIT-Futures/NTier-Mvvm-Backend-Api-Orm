using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestApp.Entity;
using TestApp.WCFService;

namespace TestApp.Tests.Moq
{
    [TestClass]
    public class MockingArchitectureTests
    {
        [TestMethod]
        public void Moq_IEntityObject_CanMockTableAndKeyMetadata()
        {
            var mockEntity = new Mock<IEntityObject>();

            mockEntity.Setup(e => e.GetTableName()).Returns("CustomTable");
            mockEntity.Setup(e => e.GetKeyName()).Returns("CustomKeyId");

            Assert.AreEqual("CustomTable", mockEntity.Object.GetTableName());
            Assert.AreEqual("CustomKeyId", mockEntity.Object.GetKeyName());

            mockEntity.Verify(e => e.GetTableName(), Times.Once);
            mockEntity.Verify(e => e.GetKeyName(), Times.Once);
        }

        [TestMethod]
        public void Moq_ICommand_VerifiesExecutionAndParameters()
        {
            var mockCommand = new Mock<ICommand>();
            object? receivedParam = null;

            mockCommand
                .Setup(c => c.CanExecute(It.IsAny<object>()))
                .Returns<object>(param => param is int val && val > 0);

            mockCommand
                .Setup(c => c.Execute(It.IsAny<object>()))
                .Callback<object>(param => receivedParam = param);

            Assert.IsFalse(mockCommand.Object.CanExecute(-1));
            Assert.IsTrue(mockCommand.Object.CanExecute(10));

            mockCommand.Object.Execute("action-payload");

            Assert.AreEqual("action-payload", receivedParam);
            mockCommand.Verify(c => c.Execute("action-payload"), Times.Once);
        }

        [TestMethod]
        public void Moq_IEntityWCFService_SetupSequence_ReturnsDifferentValuesPerCall()
        {
            var mockService = new Mock<IEntityWCFService>();

            var entity1 = new ProductEntity { ProductId = 1, Name = "First" };
            var entity2 = new ProductEntity { ProductId = 2, Name = "Second" };

            mockService
                .SetupSequence(s => s.GetById(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(entity1)
                .Returns(entity2)
                .Returns((EntityObject)null!);

            var first = mockService.Object.GetById("Type", 1);
            var second = mockService.Object.GetById("Type", 2);
            var third = mockService.Object.GetById("Type", 3);

            Assert.AreEqual("First", first.Name);
            Assert.AreEqual("Second", second.Name);
            Assert.IsNull(third);

            mockService.Verify(s => s.GetById("Type", It.IsAny<int>()), Times.Exactly(3));
        }

        [TestMethod]
        public void Moq_IEntityWCFService_CapturesInvocationArgumentsWithCallbacks()
        {
            var mockService = new Mock<IEntityWCFService>();
            var requestedTypes = new List<string>();

            mockService
                .Setup(s => s.GetAll(It.IsAny<string>()))
                .Callback<string>(typeName => requestedTypes.Add(typeName))
                .Returns(new List<EntityObject>());

            mockService.Object.GetAll("TestApp.Entity.ProductEntity");
            mockService.Object.GetAll("TestApp.Entity.EmployeeEntity");

            Assert.AreEqual(2, requestedTypes.Count);
            Assert.AreEqual("TestApp.Entity.ProductEntity", requestedTypes[0]);
            Assert.AreEqual("TestApp.Entity.EmployeeEntity", requestedTypes[1]);
        }

        [TestMethod]
        public void Moq_PropertyChangedEventHandler_VerifiesViewModelEventRaised()
        {
            var mockObserver = new Mock<PropertyChangedEventHandler>();
            var entity = new ProductEntity { ProductId = 1 };
            var vm = new TestApp.BaseUI.EntityViewModel(entity, null);

            vm.PropertyChanged += mockObserver.Object;

            vm.Entity = new ProductEntity { ProductId = 2 };

            mockObserver.Verify(
                handler => handler.Invoke(vm, It.Is<PropertyChangedEventArgs>(e => e.PropertyName == "Entity")),
                Times.Once
            );
        }

        [TestMethod]
        public void Moq_IEntityWCFService_SimulatesTimeoutException()
        {
            var mockService = new Mock<IEntityWCFService>();

            mockService
                .Setup(s => s.GetById(It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new TimeoutException("Operation timed out waiting for backend server"));

            var ex = Assert.Throws<TimeoutException>(() =>
            {
                mockService.Object.GetById("TestApp.Entity.ProductEntity", 1);
            });

            StringAssert.Contains(ex.Message, "Operation timed out");
        }

        [TestMethod]
        public void Moq_IEntityObject_PolymorphismInCollection()
        {
            var mock1 = new Mock<IEntityObject>();
            var mock2 = new Mock<IEntityObject>();

            mock1.Setup(m => m.GetTableName()).Returns("TableA");
            mock2.Setup(m => m.GetTableName()).Returns("TableB");

            var list = new List<IEntityObject> { mock1.Object, mock2.Object };

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("TableA", list[0].GetTableName());
            Assert.AreEqual("TableB", list[1].GetTableName());
        }
    }
}
