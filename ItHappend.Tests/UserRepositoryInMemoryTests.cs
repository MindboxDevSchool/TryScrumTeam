using System;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using NUnit.Framework;

namespace ItHappend.Tests
{
    public class UserRepositoryInMemoryTests
    {
        [Test]
        public void TryCreate_TryGetEventsByTrack()
        {
            var repository = new UserRepositoryInMemory();
            var id = Guid.NewGuid();
            var user = new User(id,"1","1");
            repository.TryCreate(user);
            var result = repository.TryGet("1", "1");
            
            Assert.AreEqual(result.Value,user);

        }
    }
}