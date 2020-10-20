using System;

namespace ItHappened.Domain
{
    public class AuthData
    {
        public AuthData(Guid id, string token)
        {
            Id = id;
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public readonly Guid Id;
        public readonly string Token;
    }
}