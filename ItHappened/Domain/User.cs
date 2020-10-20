using System;

namespace ItHappened.Domain
{
    public class User
    {
        public User(Guid userId, string login, string hashedPassword, string token)
        {
            Id = userId;
            Login = login ?? throw new ArgumentNullException(nameof(login));
            HashedPassword = hashedPassword ?? throw new ArgumentNullException(nameof(hashedPassword));
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public Guid Id { get; }
        public string Login { get; }
        public string HashedPassword { get; }
        public string Token { get;  }
    }
}