using System;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;
using NUnit.Framework;

namespace ItHappend.Tests
{
    public class UserServiceTests
    {
        public class UserMocRepository : IUserRepository
        {
            public Result<User> ReturnedResult { get; set; }
            public User SavedUser { get; set; }

            public Result<User> TryCreate(User user)
            {
                if (ReturnedResult == null)
                {
                    SavedUser = user;
                    return new Result<User>(user);
                }

                return ReturnedResult;
            }

            public Result<User> TryGetByLogin(string login)
            {
                return ReturnedResult;
            }

            public Result<User> TryGetByToken(string token)
            {
                throw new NotImplementedException();
            }

            public Result<User> TryGetById(Guid id)
            {
                throw new NotImplementedException();
            }

            public Result<bool> IsUserAuthDataValid(AuthData data)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void CreateUser_ForUniqueLogin_ReturnAuthData()
        {
            var login = "login";
            var password = "password";
            var repository = new UserMocRepository();
            var userService = new UserService(repository);

            var result = userService.CreateUser(login, password);

            Assert.True(result.IsSuccessful());
            Assert.AreEqual(repository.SavedUser.Login, login);
            Assert.AreEqual(result.Value.Token, repository.SavedUser.Token);
            Assert.AreEqual(result.Value.Id, repository.SavedUser.Id);
            Assert.False(String.IsNullOrEmpty(result.Value.Token));
            Assert.NotNull(result.Value.Id);
        }

        [Test]
        public void CreateUser_ForRepeatLogin_ReturnBadDataException()
        {
            var login = "login";
            var password = "password";
            var repository = new UserMocRepository();
            var userService = new UserService(repository);
            repository.ReturnedResult = new Result<User>(new Exception());
            var result = userService.CreateUser(login, password);

            Assert.False(result.IsSuccessful());
            Assert.True(result.Exception is BadDataException);
        }

        [Test]
        public void LoginUser_ForCorrectLoginIncorrectPassword_ReturnUnauthorizedException()
        {
            var login = "login";
            var password = "password";
            var repository = new UserMocRepository();
            var userService = new UserService(repository);
            userService.CreateUser(login, password);
            repository.ReturnedResult = new Result<User>(repository.SavedUser);

            var result = userService.LoginUser(login, "failed");

            Assert.False(result.IsSuccessful());
            Assert.True(result.Exception is UnauthorizedException);
        }

        [Test]
        public void LoginUser_ForInvalidLogin_ReturnUnauthorizedException()
        {
            var login = "login";
            var password = "password";
            var repository = new UserMocRepository();
            var userService = new UserService(repository);
            userService.CreateUser(login, password);
            repository.ReturnedResult = new Result<User>(new Exception());

            var result = userService.LoginUser("failed", password);

            Assert.False(result.IsSuccessful());
            Assert.True(result.Exception is UnauthorizedException);
        }

        [Test]
        public void LoginUser_ForCorrectLoginAndPassword_ReturnAuthData()
        {
            var login = "login";
            var password = "password";
            var repository = new UserMocRepository();
            var userService = new UserService(repository);
            userService.CreateUser(login, password);
            repository.ReturnedResult = new Result<User>(repository.SavedUser);

            var result = userService.LoginUser(login, password);

            Assert.True(result.IsSuccessful());
            Assert.AreEqual(result.Value.Token, repository.SavedUser.Token);
            Assert.AreEqual(result.Value.Id, repository.SavedUser.Id);
        }
    }
}