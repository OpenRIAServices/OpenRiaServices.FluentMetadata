using System.ComponentModel.DataAnnotations;
using FluentMetadata.Tests.Web.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FluentMetadata.Tests
{
    [TestClass]
    public class FluentMetadataTests
    {
        [TestMethod]
        [Description("Tests the .IsKey expression")]
        public void TestHasKeyAttribute()
        {
            var typeOfFoo = typeof(Foo);
            var propInfo = typeOfFoo.GetProperty("Id2");
            Assert.IsFalse(propInfo == null, "propInfo should not be null");
            Assert.IsTrue(propInfo.IsDefined(typeof(KeyAttribute), false), "property should have defined the KeyAttribute");            
        }
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
        [TestMethod]
        [Description("Tests functioning of the .Association() fluent metadata expression")]
        public void TestAssociationExpression()
        {
            var typeOfBar = typeof(Bar);
            var propInfo = typeOfBar.GetProperty("FooSet");
            Assert.IsFalse(propInfo == null, "propInfo should not be null");

            Assert.IsTrue(propInfo.IsDefined(typeof(AssociationAttribute), false), "property should have defined the AssociationAttribute");
            var association = propInfo.GetCustomAttributes(typeof(AssociationAttribute), false).OfType<AssociationAttribute>().Single();

            Assert.IsTrue(association.Name == "MyAssociation");

            Assert.IsTrue(association.ThisKeyMembers.ToArray()[0] == "Id2");
            Assert.IsTrue(association.ThisKeyMembers.ToArray()[1] == "Id");

            Assert.IsTrue(association.OtherKeyMembers.ToArray()[0] == "Id2");
            Assert.IsTrue(association.OtherKeyMembers.ToArray()[1] == "Id");
        }
    }
}