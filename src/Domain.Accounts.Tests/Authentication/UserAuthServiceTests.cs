using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuickShop.Domain.Accounts.Authentication;
using QuickShop.Domain.Accounts.Authentication.HashingAlgorithm;
using QuickShop.Domain.Accounts.Model.UserAggregate;

namespace QuickShop.Accounts.Tests.Authentication
{
    [TestClass]
    public class UserAuthServiceTests
    { 
        [TestMethod]
        public async Task When_UserHasNotBeenFound_Should_ReturnAuthenticationResult_With_InvalidCredentialsCode()
        {
            IUserAuthService service = CreateMockService();

            var authenticationResult = await service.AuthenticateAsync(IncorrectUsername, CorrectPassword);

            Assert.AreEqual(AuthenticationResultCode.InvalidCredentials, authenticationResult.Code);
            Assert.IsNull(authenticationResult.User);
        }

        [TestMethod]
        public async Task When_PasswordMismatches_Should_ReturnAuthenticationResult_With_InvalidCredentialsCode()
        {
            IUserAuthService service = CreateMockService();

            var authenticationResult = await service.AuthenticateAsync(CorrectUsername, IncorrectPassword);

            Assert.AreEqual(AuthenticationResultCode.InvalidCredentials, authenticationResult.Code);
            Assert.IsNull(authenticationResult.User);
        }

        [TestMethod]
        public async Task When_CredentialsAreValid_Should_ReturnAuthenticationResult_With_SuccessCodeAndUser()
        {
            IUserAuthService service = CreateMockService();

            var authenticationResult = await service.AuthenticateAsync(CorrectUsername, CorrectPassword);

            Assert.AreEqual(AuthenticationResultCode.Success, authenticationResult.Code);
            Assert.IsNotNull(authenticationResult.User);
        }

        private const string CorrectUsername = "my-username";
        private const string IncorrectUsername = "another-username";
        private const string CorrectPassword = "my-password";
        private const string IncorrectPassword = "another-password";

        private IUserAuthService CreateMockService()
        {
            IHashingAlgorithm hashingAlgorithm = new MockHashingAlgorithm();

            User user = new User(DateTimeOffset.Now, new UserCredentials(CorrectUsername, hashingAlgorithm.Hash(CorrectPassword)));

            IUserRepository repository = new MockUserRepository(user);

            return new UserAuthService(hashingAlgorithm, repository, new NullLogger());
        }

        private class MockUserRepository : IUserRepository
        {
            private readonly User _user;

            public MockUserRepository(User user)
            {
                _user = user;
            }

            public Task<User> CreateAsync(string username, string passwordHash)
            {
                throw new NotImplementedException();
            }

            public Task<User> FindByUsernameOrDefaultAsync(string username)
            {
                if (_user == null)
                    return Task.FromResult<User>(null);

                if (_user.Credentials.Username != username)
                    return Task.FromResult<User>(null);

                return Task.FromResult(_user);
            }
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
