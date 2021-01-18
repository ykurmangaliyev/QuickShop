using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickShop.Domain.Accounts.Authentication.HashingAlgorithm;

namespace QuickShop.Accounts.Tests.Authentication.HashingAlgorithm
{
    [TestClass]
    public class Sha512HashingAlgorithmTests
    { 
        [TestMethod]
        public void When_ValidStringIsGiven_Should_ProduceValidSha512Hash()
        {
            Sha512HashingAlgorithm algorithm = new Sha512HashingAlgorithm();

            string expected = "n32GJ+Avl8xaUtyyupYDj+EvKjSw+sUOBBNZrhPV7eiopQVi2li6eRbaN45zQ++R6F771qCnCrI3raTCJ03xPQ==";
            string actual = algorithm.Hash("test");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void When_WhiteSpaceStringIsGiven_Should_RaiseArgumentException()
        {
            Sha512HashingAlgorithm algorithm = new Sha512HashingAlgorithm();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                algorithm.Hash(" ");
            });
        }

        [TestMethod]
        public void When_NullStringIsGiven_Should_RaiseArgumentException()
        {
            Sha512HashingAlgorithm algorithm = new Sha512HashingAlgorithm();

            Assert.ThrowsException<ArgumentException>(() =>
            {
                algorithm.Hash(null);
            });
        }
    }
}
