using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickShop.Extensions.Security.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void When_AnyArgumentIsNull_Should_RaiseArgumentNullExceptions()
        {
            string nullString = null;
            string notNullString = "test";

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                nullString.TimeConstantEquals(notNullString);
            });

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                notNullString.TimeConstantEquals(nullString);
            });
        }

        [TestMethod]
        public void When_StringsAreEqual_Should_ReturnTrue()
        {
            string a = "test";
            string b = "test";

            bool actual = a.TimeConstantEquals(b);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void When_StringsAreNotEqual_Should_ReturnFalse()
        {
            string a = "test";
            string b = "another";

            bool actual = a.TimeConstantEquals(b);

            Assert.IsFalse(actual);
        }
    }
}
