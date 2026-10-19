using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.BaseUI;
using TestApp.Entity;

namespace TestApp.Tests.BaseUI
{
    [TestClass]
    public class EntityViewModelTests
    {
        private class TestViewModel : EntityViewModel
        {
            private string title = string.Empty;
            public string Title
            {
                get => title;
                set
                {
                    title = value;
                    OnPropertyChange(() => Title);
                }
            }

            public TestViewModel() : base() { }
            public TestViewModel(EntityObject entity) : base(entity, null) { }
        }

        [TestMethod]
        public void EntityViewModel_DefaultConstructor_EntityIsNull()
        {
            var vm = new EntityViewModel();
            Assert.IsNull(vm.Entity);
            Assert.IsNull(vm.View);
        }

        [TestMethod]
        public void EntityViewModel_ParameterizedConstructor_SetsProperties()
        {
            var entity = new ProductEntity { ProductId = 1, Name = "Laptop" };
            var vm = new EntityViewModel(entity, null);

            Assert.AreSame(entity, vm.Entity);
            Assert.IsNull(vm.View);
        }

        [TestMethod]
        public void EntityViewModel_SettingEntity_RaisesPropertyChangedEvent()
        {
            var vm = new EntityViewModel();
            var raisedEvents = new List<string?>();

            vm.PropertyChanged += (sender, args) =>
            {
                raisedEvents.Add(args.PropertyName);
            };

            var entity = new EmployeeEntity { EmployeeId = 99 };
            vm.Entity = entity;

            Assert.AreEqual(1, raisedEvents.Count);
            Assert.AreEqual("Entity", raisedEvents[0]);
            Assert.AreSame(entity, vm.Entity);
        }

        [TestMethod]
        public void EntityViewModel_DerivedViewModel_OnPropertyChange_RaisesCorrectPropertyName()
        {
            var vm = new TestViewModel();
            var propertyNames = new List<string?>();

            vm.PropertyChanged += (sender, args) =>
            {
                propertyNames.Add(args.PropertyName);
            };

            vm.Title = "New Title";

            Assert.AreEqual(1, propertyNames.Count);
            Assert.AreEqual("Title", propertyNames[0]);
            Assert.AreEqual("New Title", vm.Title);
        }
    }
}
