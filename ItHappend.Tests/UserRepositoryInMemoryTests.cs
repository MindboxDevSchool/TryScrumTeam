using System;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using NUnit.Framework;

namespace ItHappend.Tests
{
    public class UserRepositoryInMemoryTests
    {
        [Test]
        public void TryGetById()
        {
            var repository = new UserRepositoryInMemory();
            var id = Guid.NewGuid();
            var user = new User(id,"1","1", "token");
            repository.TryCreate(user);
            var result = repository.TryGetById(user.Id);
            
            Assert.AreEqual(result.Value,user);
        }
        
        [Test]
        public void TryGetByLogin()
        {
            var repository = new UserRepositoryInMemory();
            var id = Guid.NewGuid();
            var user = new User(id,"1","2", "token");
            repository.TryCreate(user);
            var result = repository.TryGetByLogin("1");
            
            Assert.AreEqual(result.Value,user);
        }
        
        [Test]
        public void IsUserAuthDataValid_ValidData_ReturnTrue()
        {
            var repository = new UserRepositoryInMemory();
            var id = Guid.NewGuid();
            var token = "token";
            var user = new User(id,"1","2", token);
            repository.TryCreate(user);
            var authData = new AuthData(id, token);
            
            var result = repository.IsUserAuthDataValid(authData);
            
            Assert.True(result);
        }

        [Test]
        public void IsUserAuthDataValid_InvalidToken_ReturnFalse()
        {
            var repository = new UserRepositoryInMemory();
            var id = Guid.NewGuid();
            var token = "token";
            var user = new User(id,"1","2", token);
            repository.TryCreate(user);
            var authData = new AuthData(id, "failed token");
            
            var result = repository.IsUserAuthDataValid(authData);
            
            Assert.False(result);
        }
        
        [Test]
        public void IsUserAuthDataValid_InvalidUser_ReturnFalse()
        {
            var repository = new UserRepositoryInMemory();
            var id = Guid.NewGuid();
            var token = "token";
            var user = new User(id,"1","2", token);
            repository.TryCreate(user);
            var authData = new AuthData(Guid.NewGuid(), token);
            
            var result = repository.IsUserAuthDataValid(authData);
            
            Assert.False(result);
        }
    }
}