using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuickShop.Domain.Accounts;
using QuickShop.Domain.Accounts.Authentication;
using QuickShop.Domain.Accounts.Authentication.HashingAlgorithm;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Repository.Abstractions;

namespace QuickShop.Accounts.Tests.Authentication
{
    [TestClass]
    public class UserAuthServiceTests
    { 
        [TestMethod]
        public async Task When_UserHasNotBeenFound_Should_ReturnAuthenticationResult_With_InvalidCredentialsCode()
        {
            IUserAuthService service = await CreateMockService();

            var authenticationResult = await service.AuthenticateAsync(IncorrectUsername, CorrectPassword);

            Assert.AreEqual(AuthenticationResultCode.InvalidCredentials, authenticationResult.Code);
            Assert.IsNull(authenticationResult.User);
        }

        [TestMethod]
        public async Task When_PasswordMismatches_Should_ReturnAuthenticationResult_With_InvalidCredentialsCode()
        {
            IUserAuthService service = await CreateMockService();

            var authenticationResult = await service.AuthenticateAsync(CorrectUsername, IncorrectPassword);

            Assert.AreEqual(AuthenticationResultCode.InvalidCredentials, authenticationResult.Code);
            Assert.IsNull(authenticationResult.User);
        }

        [TestMethod]
        public async Task When_CredentialsAreValid_Should_ReturnAuthenticationResult_With_SuccessCodeAndUser()
        {
            IUserAuthService service = await CreateMockService();

            var authenticationResult = await service.AuthenticateAsync(CorrectUsername, CorrectPassword);

            Assert.AreEqual(AuthenticationResultCode.Success, authenticationResult.Code);
            Assert.IsNotNull(authenticationResult.User);
        }

        private const string CorrectUsername = "my-username";
        private const string IncorrectUsername = "another-username";
        private const string CorrectPassword = "my-password";
        private const string IncorrectPassword = "another-password";

        private async Task<IUserAuthService> CreateMockService()
        {
            IHashingAlgorithm hashingAlgorithm = new MockHashingAlgorithm();

            IUserRepository repository = new UserRepository(new InMemoryDatabaseContext());
            await repository.CreateAsync(CorrectUsername, hashingAlgorithm.Hash(CorrectPassword));

            return new UserAuthService(hashingAlgorithm, repository, new NullLogger());
        }

        private class MockHashingAlgorithm : IHashingAlgorithm
        {
            public string Hash(string digest)
            {
                return $"plain-{digest}-string";
            }
        }
    }
}
