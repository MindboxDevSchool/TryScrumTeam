using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface IUserService
    {
        Result<UserDto> CreateUser(string login, string password);
        Result<UserDto> LoginUser(string login, string password);
    }
}