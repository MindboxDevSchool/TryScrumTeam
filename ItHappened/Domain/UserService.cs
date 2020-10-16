using System;
using ItHappened.Domain.Repositories;
using ItHappened.Infrastructure;

namespace ItHappened.Domain
{
    public class UserService : IUserService
    {
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        private readonly IUserRepository _userRepository;
        private readonly IHashingPassword _hashingPassword = new ShaHashing();

        public Result<AuthData> CreateUser(string login, string password)
        {
            var hashedPassword = _hashingPassword.HashPassword(password);
            var token = GenerateToken();
            var userId = new Guid();
            var user = new User(userId, login, hashedPassword, token);
            var result = _userRepository.TryCreate(user);
            if (result.IsSuccessful())
                return new Result<AuthData>(new AuthData(userId, token));
            return new Result<AuthData>(new BadDataException("not unique login"));
        }

        public Result<AuthData> LoginUser(string login, string password)
        {
            var result = _userRepository.TryGetByLogin(login);
            if (!result.IsSuccessful())
                return new Result<AuthData>(new UnauthorizedException("incorrect login"));

            var user = result.Value;
            if (_hashingPassword.Verify(password, user.HashedPassword))
                return new Result<AuthData>(new AuthData(user.Id, user.Token));

            return new Result<AuthData>(new UnauthorizedException("incorrect password"));
        }

        private string GenerateToken()
        {
            return new Guid().ToString();
        }
    }
}