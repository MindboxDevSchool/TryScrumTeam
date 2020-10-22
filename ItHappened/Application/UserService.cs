using System;
using ItHappened.Domain;
using ItHappened.Domain.Exceptions;
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

        public UserDto CreateUser(string login, string password)
        {
            var hashedPassword = _hashingPassword.HashPassword(password);
            var userId = Guid.NewGuid();
            var user = new User(userId, login, hashedPassword);
            var createdUser = _userRepository.TryCreate(user);
            return new UserDto(createdUser.Id, createdUser.Login);
        }

        public UserDto LoginUser(string login, string password)
        {
            var user = _userRepository.TryGetByLogin(login);
            
            if (_hashingPassword.Verify(password, user.HashedPassword))
                return new UserDto(user.Id, user.Login);

            throw new DomainException(DomainExceptionType.IncorrectPassword, login);
        }
    }
}