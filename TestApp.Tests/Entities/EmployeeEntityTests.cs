using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Entity;

namespace TestApp.Tests.Entities
{
    [TestClass]
    public class EmployeeEntityTests
    {
        [TestMethod]
        public void EmployeeEntity_GetTableName_ReturnsEmployee()
        {
            var emp = new EmployeeEntity();
            Assert.AreEqual("Employee", emp.GetTableName());
        }

        [TestMethod]
        public void EmployeeEntity_GetKeyName_ReturnsEmployeeId()
        {
            var emp = new EmployeeEntity();
            Assert.AreEqual("EmployeeId", emp.GetKeyName());
        }

        [TestMethod]
        public void EmployeeEntity_SettingEmployeeId_AlsoUpdatesBaseId()
        {
            var emp = new EmployeeEntity
            {
                EmployeeId = 77
            };

            Assert.AreEqual(77, emp.EmployeeId);
            Assert.AreEqual(77, emp.Id);
        }

        [TestMethod]
        public void EmployeeEntity_Properties_CanBeSetAndRetrieved()
        {
            var emp = new EmployeeEntity
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                Name = "John Doe",
                Description = "Software Engineer"
            };

            Assert.AreEqual(1, emp.EmployeeId);
            Assert.AreEqual("John", emp.FirstName);
            Assert.AreEqual("Doe", emp.LastName);
            Assert.AreEqual("John Doe", emp.Name);
            Assert.AreEqual("Software Engineer", emp.Description);
        }

        [TestMethod]
        public void EmployeeEntity_Validation_FirstNameNullOrEmpty_ReturnsValidationError()
        {
            var empNull = new EmployeeEntity { FirstName = null! };
            Assert.AreEqual("First Name must have a value.", empNull["FirstName"]);

            var empEmpty = new EmployeeEntity { FirstName = "" };
            Assert.AreEqual("First Name must have a value.", empEmpty["FirstName"]);
        }

        [TestMethod]
        public void EmployeeEntity_Validation_FirstNameProvided_ReturnsEmpty()
        {
            var emp = new EmployeeEntity { FirstName = "Jane" };
            Assert.AreEqual(string.Empty, emp["FirstName"]);
        }

        [TestMethod]
        public void EmployeeEntity_Validation_LastNameNullOrEmpty_ReturnsValidationError()
        {
            var empNull = new EmployeeEntity { LastName = null! };
            Assert.AreEqual("Last Name must have a value.", empNull["LastName"]);

            var empEmpty = new EmployeeEntity { LastName = "" };
            Assert.AreEqual("Last Name must have a value.", empEmpty["LastName"]);
        }

        [TestMethod]
        public void EmployeeEntity_Validation_LastNameProvided_ReturnsEmpty()
        {
            var emp = new EmployeeEntity { LastName = "Smith" };
            Assert.AreEqual(string.Empty, emp["LastName"]);
        }

        [TestMethod]
        public void EmployeeEntity_Validation_InheritedPropertiesValidation_WorksProperly()
        {
            var emp = new EmployeeEntity { Id = -1, Name = "" };
            Assert.AreEqual("Id must be greater than 0!", emp["Id"]);
            Assert.AreEqual("Name is required!", emp["Name"]);
        }

        [TestMethod]
        public void EmployeeEntity_ToString_ContainsExpectedTokens()
        {
            var emp = new EmployeeEntity
            {
                EmployeeId = 15,
                FirstName = "Alice",
                LastName = "Wonderland",
                Name = "Alice W.",
                Description = "Developer"
            };

            var str = emp.ToString();
            StringAssert.Contains(str, "Alice");
            StringAssert.Contains(str, "Wonderland");
            StringAssert.Contains(str, "Table=Employee");
            StringAssert.Contains(str, "Key=EmployeeId");
        }

        [TestMethod]
        public void EmployeeEntity_ErrorProperty_ReturnsError()
        {
            var emp = new EmployeeEntity();
            Assert.AreEqual("Error", emp.Error);
        }
    }
}
