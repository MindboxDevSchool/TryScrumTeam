﻿using System;
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
            var user = new User(id,"1","1");
            repository.TryCreate(user);
            var result = repository.TryGetById(user.Id);
            
            Assert.AreEqual(result,user);
        }
        
        [Test]
        public void TryGetByLogin()
        {
            var repository = new UserRepositoryInMemory();
            var id = Guid.NewGuid();
            var user = new User(id,"1","2");
            repository.TryCreate(user);
            var result = repository.TryGetByLogin("1");
            
            Assert.AreEqual(result,user);
        }
    }
}