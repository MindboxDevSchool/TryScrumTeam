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
            _loginString = "login";
            _passwordString = "password";
            _testUser = new User(_testUserId, _loginString, shaHashing.HashPassword(_passwordString));
        }

        private void SetupMoqUserRepository()
        {
            var mock = new Mock<IUserRepository>();
            mock.Setup(method => method.TryCreate(
                    It.Is<User>(a => _testUser.Login == a.Login && _testUser.HashedPassword == a.HashedPassword)))
                .Returns(_testUser);
            mock.Setup(method => method.TryGetByLogin(_loginString))
                .Returns(_testUser);

            _userRepository = mock.Object;
        }

        private User _testUser;
        private Guid _testUserId;
        private IUserRepository _userRepository;
        private string _loginString;
        private string _passwordString;

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

            var result = userService.CreateUser(_loginString, _passwordString);

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
                var result = userService.LoginUser(_loginString, "incorrectPassword");
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
            
            var result = userService.LoginUser(_loginString, _passwordString);
            
            Assert.AreEqual(_testUser.Id, result.Id);
            Assert.AreEqual(_testUser.Login, result.Login);
        }
    }
}