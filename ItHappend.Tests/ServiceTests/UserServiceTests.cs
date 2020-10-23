using System;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;
using ItHappened.Infrastructure;
using NUnit.Framework;
using Moq;

namespace ItHappend.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private void SetupEntities()
        {
            _testUserId = Guid.NewGuid();
            var shaHashing = new ShaHashing();
            _testUser = new User(_testUserId, "login", shaHashing.HashPassword("password"));
        }

        private void SetupMoqUserRepository()
        {
            var mock = new Mock<IUserRepository>();
            mock.Setup(method => method.TryCreate(It.IsAny<User>()))
                .Returns(_testUser);
            mock.Setup(method => method.TryGetByLogin(It.IsAny<string>()))
                .Returns(_testUser);

            _userRepository = mock.Object;
        }

        private User _testUser;
        private Guid _testUserId;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            SetupEntities();
            SetupMoqUserRepository();
        }

        [Test]
        public void CreateUser_ForUniqueLogin_ReturnUserDto()
        {
            var userService = new UserService(_userRepository);

            var result = userService.CreateUser("login", "password");

            Assert.AreEqual(_testUser.Id, result.Id);
            Assert.AreEqual(_testUser.Login, result.Login);
        }

        [Test]
        public void LoginUser_ForCorrectLoginIncorrectPassword_ThrowException()
        {
            var userService = new UserService(_userRepository);
            DomainException exception = null;

            try
            {
                var result = userService.LoginUser("login", "incorrectPassword");
            }
            catch (DomainException e)
            {
                exception = e;
            }

            Assert.AreEqual(DomainExceptionType.IncorrectPassword, exception.Type);
        }

        [Test]
        public void LoginUser_ForCorrectLoginAndPassword_ReturnUserDto()
        {
            var userService = new UserService(_userRepository);
            
            var result = userService.LoginUser("login", "password");
            
            Assert.AreEqual(_testUser.Id, result.Id);
            Assert.AreEqual(_testUser.Login, result.Login);
        }
    }
}