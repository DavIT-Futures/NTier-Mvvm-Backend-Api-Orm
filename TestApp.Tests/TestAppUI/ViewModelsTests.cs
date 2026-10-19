using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestApp.Entity;
using TestApp.TestAppUI.ViewModel;

namespace TestApp.Tests.TestAppUI
{
    [TestClass]
    public class ViewModelsTests
    {
        [TestMethod]
        public void ProductViewModel_DisplayCommand_IsInitializedProperly()
        {
            var product = new ProductEntity { ProductId = 1, Name = "Laptop" };
            var vm = new ProductViewModel(product, null);

            Assert.IsNotNull(vm.DisplayCommand);
            Assert.AreSame(product, vm.Entity);
        }

        [TestMethod]
        public void ProductViewModel_CanDisplay_ChecksEntityError()
        {
            var product = new ProductEntity { ProductId = 1 };
            var vm = new ProductViewModel(product, null);

            // Entity.Error returns "Error" by default in EntityObject, so string.IsNullOrEmpty is false
            bool canDisplay = vm.CanDisplay(null);
            Assert.IsFalse(canDisplay);
        }

        [TestMethod]
        public void EmployeeViewModel_DisplayCommand_IsInitializedProperly()
        {
            var employee = new EmployeeEntity { EmployeeId = 5, FirstName = "John", LastName = "Doe" };
            var vm = new EmployeeViewModel(employee, null);

            Assert.IsNotNull(vm.DisplayCommand);
            Assert.IsTrue(vm.CanDisplay(null));
            Assert.AreSame(employee, vm.Entity);
        }

        [TestMethod]
        public void MainViewModel_CanOpenProduct_DependsOnSelectedProduct()
        {
            var vm = new MainViewModel(null, null);

            Assert.IsFalse(vm.CanOpenProduct(null));

            vm.SelectedProduct = new ProductEntity { ProductId = 1 };
            Assert.IsTrue(vm.CanOpenProduct(null));
        }

        [TestMethod]
        public void MainViewModel_CanOpenEmployee_DependsOnSelectedEmployee()
        {
            var vm = new MainViewModel(null, null);

            Assert.IsFalse(vm.CanOpenEmployee(null));

            vm.SelectedEmployee = new EmployeeEntity { EmployeeId = 2 };
            Assert.IsTrue(vm.CanOpenEmployee(null));
        }

        [TestMethod]
        public void MainViewModel_Properties_RaisePropertyChanged()
        {
            var vm = new MainViewModel(null, null);
            var changedProps = new List<string>();

            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != null)
                    changedProps.Add(e.PropertyName);
            };

            vm.Products = new List<ProductEntity>();
            vm.Employees = new List<EmployeeEntity>();
            vm.ProgressValue = 75;
            vm.ProgressColor = Colors.Green;

            CollectionAssert.Contains(changedProps, "Products");
            CollectionAssert.Contains(changedProps, "Employees");
            CollectionAssert.Contains(changedProps, "ProgressValue");
            CollectionAssert.Contains(changedProps, "ProgressColor");
        }

        [TestMethod]
        public void MainViewModel_Commands_CanExecuteByDefault()
        {
            var vm = new MainViewModel(null, null);

            Assert.IsTrue(vm.CanOpenCovariance(null));
            Assert.IsTrue(vm.CanSearchEntitiesCommand(null));
            Assert.IsTrue(vm.CanAnonymousMethodsCommand(null));
            Assert.IsTrue(vm.CanTest(null));
        }

        [TestMethod]
        public void MainViewModel_DoAnonymousMethodsCommand_ExecutesSuccessfully()
        {
            var vm = new MainViewModel(null, null);
            // Tests execution of delegates, anonymous methods, lambdas, Action, Predicate, Func
            vm.DoAnonymousMethodsCommand(null);
        }

        [TestMethod]
        public void MainViewModel_DoTest_ExecutesWithoutException()
        {
            var vm = new MainViewModel(null, null);
            vm.DoTest(null);
        }
    }
}
