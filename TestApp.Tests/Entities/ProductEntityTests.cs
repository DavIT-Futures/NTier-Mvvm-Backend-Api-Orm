using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Entity;

namespace TestApp.Tests.Entities
{
    [TestClass]
    public class ProductEntityTests
    {
        [TestMethod]
        public void ProductEntity_GetTableName_ReturnsProduct()
        {
            var product = new ProductEntity();
            Assert.AreEqual("Product", product.GetTableName());
        }

        [TestMethod]
        public void ProductEntity_GetKeyName_ReturnsProductId()
        {
            var product = new ProductEntity();
            Assert.AreEqual("ProductId", product.GetKeyName());
        }

        [TestMethod]
        public void ProductEntity_SettingProductId_AlsoUpdatesBaseId()
        {
            var product = new ProductEntity
            {
                ProductId = 100
            };

            Assert.AreEqual(100, product.ProductId);
            Assert.AreEqual(100, product.Id);
        }

        [TestMethod]
        public void ProductEntity_Properties_CanBeSetAndRetrieved()
        {
            var date = new DateTime(2025, 5, 20);
            var product = new ProductEntity
            {
                ProductId = 5,
                Number = 1234,
                ProductType = "Hardware",
                ReleaseDate = date,
                Name = "Widget",
                Description = "A test widget"
            };

            Assert.AreEqual(5, product.ProductId);
            Assert.AreEqual(1234, product.Number);
            Assert.AreEqual("Hardware", product.ProductType);
            Assert.AreEqual(date, product.ReleaseDate);
            Assert.AreEqual("Widget", product.Name);
            Assert.AreEqual("A test widget", product.Description);
        }

        [TestMethod]
        public void ProductEntity_Validation_NumberZeroOrNegative_ReturnsValidationError()
        {
            var productZero = new ProductEntity { Number = 0 };
            Assert.AreEqual("Number must be greater than 0!", productZero["Number"]);

            var productNeg = new ProductEntity { Number = -1 };
            Assert.AreEqual("Number must be greater than 0!", productNeg["Number"]);
        }

        [TestMethod]
        public void ProductEntity_Validation_NumberPositive_ReturnsEmpty()
        {
            var product = new ProductEntity { Number = 42 };
            Assert.AreEqual(string.Empty, product["Number"]);
        }

        [TestMethod]
        public void ProductEntity_Validation_ProductTypeContainsLetterA_ReturnsValidationError()
        {
            var productWithA = new ProductEntity { ProductType = "Hardware" }; // contains 'a'
            Assert.AreEqual("Product type cannot contain 'a' letter", productWithA["ProductType"]);

            var productUpperA = new ProductEntity { ProductType = "Part" }; // contains 'a'
            Assert.AreEqual("Product type cannot contain 'a' letter", productUpperA["ProductType"]);
        }

        [TestMethod]
        public void ProductEntity_Validation_ProductTypeWithoutLetterA_ReturnsEmpty()
        {
            var product = new ProductEntity { ProductType = "Device" };
            Assert.AreEqual(string.Empty, product["ProductType"]);
        }

        [TestMethod]
        public void ProductEntity_Validation_InheritedBaseValidation_WorksProperly()
        {
            var product = new ProductEntity { Name = "" };
            Assert.AreEqual("Name is required!", product["Name"]);

            product.Name = "Valid Product";
            Assert.AreEqual(string.Empty, product["Name"]);
        }

        [TestMethod]
        public void ProductEntity_ToString_ContainsExpectedTokens()
        {
            var date = new DateTime(2026, 1, 1);
            var product = new ProductEntity
            {
                ProductId = 10,
                Number = 999,
                ProductType = "TypeB",
                ReleaseDate = date,
                Name = "Special Item",
                Description = "Top quality"
            };

            var str = product.ToString();
            StringAssert.Contains(str, "10");
            StringAssert.Contains(str, "Special Item");
            StringAssert.Contains(str, "Top quality");
            StringAssert.Contains(str, "999");
            StringAssert.Contains(str, "TypeB");
            StringAssert.Contains(str, "Table=Product");
            StringAssert.Contains(str, "Key=ProductId");
        }

        [TestMethod]
        public void ProductEntity_ErrorProperty_ReturnsError()
        {
            var product = new ProductEntity();
            Assert.AreEqual("Error", product.Error);
        }
    }
}
