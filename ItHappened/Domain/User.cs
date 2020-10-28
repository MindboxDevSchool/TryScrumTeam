using System;

namespace ItHappened.Domain
{
    public class User
    {
        public User()
        {
        }

        public User(Guid userId, string login, string hashedPassword)
        {
            Id = userId;
            Login = login ?? throw new ArgumentNullException(nameof(login));
            HashedPassword = hashedPassword ?? throw new ArgumentNullException(nameof(hashedPassword));
        }

        public Guid Id { get; }
        public string Login { get; }
        public string HashedPassword { get; }
    }
}