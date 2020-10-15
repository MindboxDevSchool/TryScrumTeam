using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure
{
    public class UserRepositoryInMemory:IUserRepository
    {
        private Dictionary<KeyValuePair<string, string>, User> _users =
            new Dictionary<KeyValuePair<string, string>, User>();
        
        public Result<User> TryCreate(User user)
        {
            var key = new KeyValuePair<string, string>(user.Login,user.HashedPassword);
            _users[key] = user;
            return new Result<User>(user);
        }

        public Result<User> TryGet(string login, string hashedPassword)
        {
            var key = new KeyValuePair<string, string>(login, hashedPassword);
            var user = _users[key];
            return new Result<User>(user);
        }
    }
}