using System.ComponentModel.DataAnnotations;
using FluentMetadata.Tests.Web.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentMetadata.Tests
{
    [TestClass]
    public class FluentMetadataTests
    {
        [TestMethod]
        [Description("Tests functioning of the .Exclude() fluent metadata expression")]
        public void TestExcludeFluentMetadataExpression()
        {
            var typeOfFoo = typeof(Foo);
            var propInfo = typeOfFoo.GetProperty("ExcludedString");
            Assert.IsTrue(propInfo == null, "propInfo should be null");
        }
        [TestMethod]
        [Description("Tests functioning of the .Required() fluent metadata expression")]
        public void TestRequiredFluentMetadataExpression()
        {
            var typeOfFoo = typeof(Foo);
            var propInfo = typeOfFoo.GetProperty("RequiredString");
            Assert.IsFalse(propInfo == null, "propInfo should not be null");

            Assert.IsTrue(propInfo.IsDefined(typeof(RequiredAttribute), false), "property should have defined the RequiredAttribute");
        }
        [TestMethod]
        [Description("Tests functioning of the .RegularExpression() fluent metadata expression")]
        public void TestRegularExpressionFluentMetadataExpression()
        {
            var typeOfFoo = typeof(Foo);
            var propInfo = typeOfFoo.GetProperty("RegularExpressionString");
            Assert.IsFalse(propInfo == null, "propInfo should not be null");

            Assert.IsTrue(propInfo.IsDefined(typeof(RegularExpressionAttribute), false), "property should have defined the RegularExpressionAttribute");
        }
    }
}