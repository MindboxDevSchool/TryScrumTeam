using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure
{
    public class UserRepositoryInMemory : IUserRepository
    {
        private Dictionary<Guid, User> _users = new Dictionary<Guid, User>();

        public Result<User> TryCreate(User user)
        {
            _users[user.Id] = user;
            if (_users.Any(elem => elem.Value.Login == user.Login))
                return new Result<User>(new Exception());
            return new Result<User>(user);
        }

        public Result<User> TryGetByLogin(string login)
        {
            var result = _users.FirstOrDefault(elem => elem.Value.Login == login);
            if (result.Equals(default(KeyValuePair<Guid, User>)))
                return new Result<User>(new Exception());
            return new Result<User>(result.Value);
        }

        public Result<User> TryGetById(Guid id)
        {
            if (_users.ContainsKey(id))
            {
                var user = _users[id];
                return new Result<User>(user);
            }

            return new Result<User>(new Exception());
        }

        public bool IsUserAuthDataValid(AuthData data)
        {
            if (_users.ContainsKey(data.Id))
            {
                return _users[data.Id].Token == data.Token;
            }

            return false;
        }
    }
}