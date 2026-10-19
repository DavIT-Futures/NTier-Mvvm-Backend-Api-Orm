using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Entity;
using TestApp.Util;

namespace TestApp.Tests.Util
{
    [TestClass]
    public class ExtensionTests
    {
        private class SampleClass
        {
            public string StringProp { get; set; } = "test";
            public int IntProp { get; set; } = 123;
            public DateTime DateProp { get; set; } = DateTime.Now;
            public bool BoolProp { get; set; } = true;
            public string SomeMethod() => "method";
        }

        [TestMethod]
        public void GetPropertyName_ReferenceTypeProperty_ReturnsCorrectName()
        {
            var sample = new SampleClass();
            Expression<Func<string>> expr = () => sample.StringProp;

            string name = expr.GetPropertyName();
            Assert.AreEqual("StringProp", name);
        }

        [TestMethod]
        public void GetPropertyName_ValueTypeProperty_ReturnsCorrectName()
        {
            var sample = new SampleClass();
            Expression<Func<int>> expr = () => sample.IntProp;

            string name = expr.GetPropertyName();
            Assert.AreEqual("IntProp", name);
        }

        [TestMethod]
        public void GetPropertyName_DateTimeProperty_ReturnsCorrectName()
        {
            var sample = new SampleClass();
            Expression<Func<DateTime>> expr = () => sample.DateProp;

            string name = expr.GetPropertyName();
            Assert.AreEqual("DateProp", name);
        }

        [TestMethod]
        public void GetPropertyName_BooleanProperty_ReturnsCorrectName()
        {
            var sample = new SampleClass();
            Expression<Func<bool>> expr = () => sample.BoolProp;

            string name = expr.GetPropertyName();
            Assert.AreEqual("BoolProp", name);
        }

        [TestMethod]
        public void GetPropertyName_BoxedUnaryExpression_ReturnsCorrectName()
        {
            var sample = new SampleClass();
            // Boxing int to object creates a UnaryExpression (Convert) around the MemberExpression
            Expression<Func<object>> expr = () => sample.IntProp;

            string name = expr.GetPropertyName();
            Assert.AreEqual("IntProp", name);
        }

        [TestMethod]
        public void GetPropertyName_EntityObjectProperties_ReturnsExactNames()
        {
            var product = new ProductEntity();

            Expression<Func<int>> idExpr = () => product.Id;
            Expression<Func<string>> nameExpr = () => product.Name;
            Expression<Func<string>> descExpr = () => product.Description;
            Expression<Func<string>> typeExpr = () => product.ProductType;
            Expression<Func<int>> numberExpr = () => product.Number;

            Assert.AreEqual("Id", idExpr.GetPropertyName());
            Assert.AreEqual("Name", nameExpr.GetPropertyName());
            Assert.AreEqual("Description", descExpr.GetPropertyName());
            Assert.AreEqual("ProductType", typeExpr.GetPropertyName());
            Assert.AreEqual("Number", numberExpr.GetPropertyName());
        }

        [TestMethod]
        public void GetPropertyName_NullExpression_ThrowsArgumentNullException()
        {
            Expression<Func<string>>? nullExpr = null;
            Assert.Throws<ArgumentNullException>(() => nullExpr!.GetPropertyName());
        }

        [TestMethod]
        public void GetPropertyName_MethodCallExpression_ThrowsArgumentException()
        {
            var sample = new SampleClass();
            Expression<Func<string>> expr = () => sample.SomeMethod();

            Assert.Throws<ArgumentException>(() => expr.GetPropertyName());
        }

        [TestMethod]
        public void GetPropertyName_ConstantExpression_ThrowsArgumentException()
        {
            Expression<Func<int>> expr = () => 42;
            Assert.Throws<ArgumentException>(() => expr.GetPropertyName());
        }
    }
}
