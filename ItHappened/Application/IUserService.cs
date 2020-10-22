using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface IUserService
    {
        UserDto CreateUser(string login, string password);
        UserDto LoginUser(string login, string password);
    }
}