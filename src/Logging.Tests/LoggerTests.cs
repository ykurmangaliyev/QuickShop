using System.Collections.Generic;
using Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickShop.Logging.Tests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void When_CalledMultipleTimes_Should_RegisterAllCalls()
        {
            MockLogger logger = new MockLogger();

            logger.Info("test-1");
            logger.Warning("test-2");
            logger.Error("test-3");

            Assert.AreEqual(3, logger.HitCount);
        }

        [TestMethod]
        public void When_CalledUsingHelperMethods_Should_SetCorrectLevel()
        {
            MockLogger logger = new MockLogger();

            logger.Verbose("test");
            Assert.AreEqual(LogLevel.Verbose, logger.Level);

            logger.Info("test");
            Assert.AreEqual(LogLevel.Info, logger.Level);

            logger.Warning("test");
            Assert.AreEqual(LogLevel.Warning, logger.Level);

            logger.Error("test");
            Assert.AreEqual(LogLevel.Error, logger.Level);
        }

        private class MockLogger : Logger
        {
            public LogLevel Level { get; private set; }

            public int HitCount { get; private set; }

            public override void Log(LogLevel level, string message)
            {
                Level = level;
                HitCount++;
            }
        }
    }
}
