using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Entity;

namespace TestApp.Tests.Entities
{
    [TestClass]
    public class EntityObjectTests
    {
        [TestMethod]
        public void EntityObject_Properties_CanBeSetAndRetrieved()
        {
            var entity = new EntityObject
            {
                Id = 42,
                Name = "Test Entity",
                Description = "A description for testing"
            };

            Assert.AreEqual(42, entity.Id);
            Assert.AreEqual("Test Entity", entity.Name);
            Assert.AreEqual("A description for testing", entity.Description);
        }

        [TestMethod]
        public void EntityObject_ToString_FormatsCorrectly()
        {
            var entity = new EntityObject
            {
                Id = 1,
                Name = "Item 1",
                Description = "Desc 1"
            };

            var str = entity.ToString();
            Assert.AreEqual("1, Item 1, Desc 1", str);
        }

        [TestMethod]
        public void EntityObject_Validation_ErrorProperty_ReturnsError()
        {
            var entity = new EntityObject();
            Assert.AreEqual("Error", entity.Error);
        }

        [TestMethod]
        public void EntityObject_Validation_IdZeroOrNegative_ReturnsValidationError()
        {
            var entityZero = new EntityObject { Id = 0 };
            Assert.AreEqual("Id must be greater than 0!", entityZero["Id"]);

            var entityNegative = new EntityObject { Id = -5 };
            Assert.AreEqual("Id must be greater than 0!", entityNegative["Id"]);
        }

        [TestMethod]
        public void EntityObject_Validation_IdPositive_ReturnsEmpty()
        {
            var entity = new EntityObject { Id = 10 };
            Assert.AreEqual(string.Empty, entity["Id"]);
        }

        [TestMethod]
        public void EntityObject_Validation_NameNullOrEmpty_ReturnsValidationError()
        {
            var entityNull = new EntityObject { Name = null! };
            Assert.AreEqual("Name is required!", entityNull["Name"]);

            var entityEmpty = new EntityObject { Name = "" };
            Assert.AreEqual("Name is required!", entityEmpty["Name"]);
        }

        [TestMethod]
        public void EntityObject_Validation_NameProvided_ReturnsEmpty()
        {
            var entity = new EntityObject { Name = "Valid Name" };
            Assert.AreEqual(string.Empty, entity["Name"]);
        }

        [TestMethod]
        public void EntityObject_Validation_DescriptionNullOrEmpty_ReturnsValidationError()
        {
            var entityNull = new EntityObject { Description = null! };
            Assert.AreEqual("Description is required!", entityNull["Description"]);

            var entityEmpty = new EntityObject { Description = "" };
            Assert.AreEqual("Description is required!", entityEmpty["Description"]);
        }

        [TestMethod]
        public void EntityObject_Validation_DescriptionProvided_ReturnsEmpty()
        {
            var entity = new EntityObject { Description = "Valid Description" };
            Assert.AreEqual(string.Empty, entity["Description"]);
        }

        [TestMethod]
        public void EntityObject_Validation_UnknownColumn_ReturnsEmpty()
        {
            var entity = new EntityObject();
            Assert.AreEqual(string.Empty, entity["NonExistentColumn"]);
        }

        [TestMethod]
        public void EntityObject_BaseGetTableName_ThrowsNotImplementedException()
        {
            var entity = new EntityObject();
            Assert.Throws<NotImplementedException>(() => entity.GetTableName());
        }

        [TestMethod]
        public void EntityObject_BaseGetKeyName_ThrowsNotImplementedException()
        {
            var entity = new EntityObject();
            Assert.Throws<NotImplementedException>(() => entity.GetKeyName());
        }

        [TestMethod]
        public void EntityObject_GetKnownTypes_ReturnsDerivedEntityTypes()
        {
            var method = typeof(EntityObject).GetMethod("GetKnownTypes", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(method, "GetKnownTypes method should exist on EntityObject");

            var result = method.Invoke(null, null) as System.Collections.IEnumerable;
            Assert.IsNotNull(result);

            var types = new System.Collections.Generic.List<Type>();
            foreach (Type t in result)
            {
                types.Add(t);
            }

            CollectionAssert.Contains(types, typeof(ProductEntity));
            CollectionAssert.Contains(types, typeof(EmployeeEntity));
        }
    }
}
