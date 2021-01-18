using System;
using System.IO;
using System.Text.RegularExpressions;
using Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickShop.Logging.Tests
{
    [TestClass]
    public class ConsoleLoggerTests
    {
        /// It is quite difficult to mock system DateTime in C#, so we do not test this part,
        /// but only test that the output matches the expected regular expression pattern
        [TestMethod]
        public void When_LogIsCalled_Should_ProductOutputToStdout()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                ConsoleLogger logger = new ConsoleLogger();
                logger.Info("something something");

                string actual = sw.ToString();

                // Example: [2021-01-16T19:18:04] [INFO] something something
                string expectedRegex = "\\[\\d{4}\\-\\d{2}\\-\\d{2}T\\d{2}:\\d{2}:\\d{2}\\] \\[INFO\\] something something";

                Assert.IsTrue(Regex.IsMatch(actual, expectedRegex));
            }
        }
    }
}