using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure.Repositories
{
    public class UserRepositoryInMemory : IUserRepository
    {
        private Dictionary<Guid, User> _users = new Dictionary<Guid, User>();

        public User TryCreate(User user)
        {
            if (_users.Any(elem => elem.Value.Login == user.Login))
                throw new RepositoryException(RepositoryExceptionType.LoginAlreadyExists, user.Login);
            _users[user.Id] = user;
            return user;
        }

        public User TryGetByLogin(string login)
        {
            var result = _users.FirstOrDefault(elem => elem.Value.Login == login);
            
            if (result.Equals(default(KeyValuePair<Guid, User>)))
                throw new RepositoryException(RepositoryExceptionType.UserNotFound, login);
            
            return result.Value;
        }

        public User TryGetById(Guid id)
        {
            if (_users.ContainsKey(id))
            {
                var user = _users[id];
                return user;
            }

            throw new RepositoryException(RepositoryExceptionType.UserNotFound, id);
        }
    }
}