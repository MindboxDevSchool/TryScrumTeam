using System;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;
using ItHappened.Infrastructure;

namespace ItHappened.Application
{
    public class UserService : IUserService
    {
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        private readonly IUserRepository _userRepository;
        private readonly IHashingPassword _hashingPassword = new ShaHashing();

        public Result<UserDto> CreateUser(string login, string password)
        {
            var hashedPassword = _hashingPassword.HashPassword(password);
            var userId = Guid.NewGuid();
            var user = new User(userId, login, hashedPassword);
            var result = _userRepository.TryCreate(user);
            if (result.IsSuccessful())
                return new Result<UserDto>(new UserDto(userId, login));
            return new Result<UserDto>(new BadDataException("not unique login"));
        }

        public Result<UserDto> LoginUser(string login, string password)
        {
            var result = _userRepository.TryGetByLogin(login);
            if (!result.IsSuccessful())
                return new Result<UserDto>(new UnauthorizedException("incorrect login"));

            var user = result.Value;
            if (_hashingPassword.Verify(password, user.HashedPassword))
                return new Result<UserDto>(new UserDto(user.Id, user.Login));

            return new Result<UserDto>(new UnauthorizedException("incorrect password"));
        }
    }
}