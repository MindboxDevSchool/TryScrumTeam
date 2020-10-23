using System;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;
using NUnit.Framework;

namespace ItHappend.Tests.ServiceTests
{
    public class UserServiceTests
    {
        public class UserMocRepository : IUserRepository
        {
            public User SavedUser { get; set; }

            public User TryCreate(User user)
            {
                if (SavedUser == null)
                {
                    SavedUser = user;
                    return user;
                }

                throw new RepositoryException(RepositoryExceptionType.LoginAlreadyExists, user.Login);
            }

            public User TryGetByLogin(string login)
            {
                if (SavedUser == null)
                    throw new RepositoryException(RepositoryExceptionType.UserNotFoundByLogin, login);
                
                return SavedUser;
            }

            public User TryGetById(Guid id)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void CreateUser_ForUniqueLogin_ReturnUserDto()
        {
            var login = "login";
            var password = "password";
            var repository = new UserMocRepository();
            var userService = new UserService(repository);

            var result = userService.CreateUser(login, password);
            
            Assert.AreEqual(result.Login, login);
            Assert.AreEqual(result.Id, repository.SavedUser.Id);
            Assert.NotNull(result.Id);
        }

        [Test]
        public void CreateUser_ForRepeatLogin_ThrowException()
        {
            var login = "login";
            var password = "password";
            var repository = new UserMocRepository();
            var userService = new UserService(repository);
            userService.CreateUser(login, password);
            RepositoryException exception = null;

            try
            {
                var result = userService.CreateUser(login, password);
            }
            catch (RepositoryException e)
            {
                exception = e;
            }

            Assert.AreEqual(RepositoryExceptionType.LoginAlreadyExists, exception.Type);
        }

        [Test]
        public void LoginUser_ForCorrectLoginIncorrectPassword_ThrowException()
        {
            var login = "login";
            var password = "password";
            var repository = new UserMocRepository();
            var userService = new UserService(repository);
            userService.CreateUser(login, password);
            DomainException exception = null;

            try
            {
                var result = userService.LoginUser(login, "failed");
            }
            catch (DomainException e)
            {
                exception = e;
            }

            Assert.AreEqual(DomainExceptionType.IncorrectPassword, exception.Type);
        }

        [Test]
        public void LoginUser_ForInvalidLogin_ReturnUnauthorizedException()
        {
            var repository = new UserMocRepository();
            var userService = new UserService(repository);
            RepositoryException exception = null;

            try
            {
                var result = userService.LoginUser("login", "password");
            }
            catch (RepositoryException e)
            {
                exception = e;
            }
            
            Assert.AreEqual(RepositoryExceptionType.UserNotFoundByLogin, exception.Type);
        }

        [Test]
        public void LoginUser_ForCorrectLoginAndPassword_ReturnUserDto()
        {
            var login = "login";
            var password = "password";
            var repository = new UserMocRepository();
            var userService = new UserService(repository);
            userService.CreateUser(login, password);

            var result = userService.LoginUser(login, password);
            
            Assert.AreEqual(result.Login, login);
            Assert.AreEqual(result.Id, repository.SavedUser.Id);
        }
    }
}