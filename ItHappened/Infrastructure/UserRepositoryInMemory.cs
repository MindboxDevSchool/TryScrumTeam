using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure
{
    public class UserRepositoryInMemory:IUserRepository
    {
        private Dictionary<Guid, User> _users = new Dictionary<Guid, User>();
        
        public Result<User> TryCreate(User user)
        {
            _users[user.Id] = user;
            return new Result<User>(user);
        }

        public Result<User> TryGetByLogin(string login)
        {
            var user = _users.First(elem => elem.Value.Login == login).Value;
            return new Result<User>(user);
        }

        public Result<User> TryGetByToken(string token)
        {
            var user = _users.First(elem => elem.Value.Token == token).Value;
            return new Result<User>(user);
        }

        public Result<User> TryGetById(Guid id)
        {
            var user = _users[id];
            return new Result<User>(user);
        }
    }
}