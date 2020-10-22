using System;

namespace ItHappened.Domain
{
    public class UserDto
    {
        public UserDto(Guid id, string login)
        {
            Id = id;
            Login = login ?? throw new ArgumentNullException(nameof(login));
        }

        public readonly Guid Id;
        public readonly string Login;
    }
}