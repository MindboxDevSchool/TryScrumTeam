using System;
using Dapper;

namespace ItHappened.Domain
{
    public class User
    {
        public User(Guid id, string login, string hashedPassword)
        {
            Id = id;
            Login = login ?? throw new ArgumentNullException(nameof(login));
            HashedPassword = hashedPassword ?? throw new ArgumentNullException(nameof(hashedPassword));
        }
        public Guid Id { get; }
        public string Login { get; }
        public string HashedPassword { get; }
    }
}