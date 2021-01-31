using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuickShop.Repository.Abstractions;

namespace QuickShop.Domain.Ping.Tests
{
    [TestClass]
    public class PingCheckTests
    {
        [TestMethod]
        public async Task When_CallPing_And_DatabaseIsUp_Should_ReturnSuccessfulPing()
        {
            var dbContextMock = new Mock<IDatabaseContext>(MockBehavior.Strict);
            dbContextMock.Setup(databaseContext => databaseContext.PingAsync()).ReturnsAsync(30.0);

            var pingService = new PingService(dbContextMock.Object);

            var pingCheckResponse = await pingService.Ping();

            Assert.IsNotNull(pingCheckResponse.ServerTime);
            Assert.AreEqual(30.0, pingCheckResponse.DatabasePing);
            Assert.IsTrue(pingCheckResponse.DatabaseStatus);
        }

        [TestMethod]
        public async Task When_CallPing_And_DatabaseIsDown_Should_ReturnUnsuccessfulPing()
        {
            var dbContextMock = new Mock<IDatabaseContext>(MockBehavior.Strict);
            dbContextMock.Setup(databaseContext => databaseContext.PingAsync()).ReturnsAsync((double?)null);

            var pingService = new PingService(dbContextMock.Object);

            var pingCheckResponse = await pingService.Ping();

            Assert.IsNotNull(pingCheckResponse.ServerTime);
            Assert.IsNull(pingCheckResponse.DatabasePing);
            Assert.IsFalse(pingCheckResponse.DatabaseStatus);
        }
    }
}
