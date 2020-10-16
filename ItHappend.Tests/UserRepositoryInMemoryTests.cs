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
        public void TryGetByToken()
        {
            var repository = new UserRepositoryInMemory();
            var id = Guid.NewGuid();
            var user = new User(id,"1","1", "token");
            repository.TryCreate(user);
            var result = repository.TryGetByToken("token");
            
            Assert.AreEqual(result.Value,user);
        }
    }
}