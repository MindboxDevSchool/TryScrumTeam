using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Application
{
    public interface IUserService
    {
        Result<AuthData> CreateUser(string login, string password);
        Result<AuthData> LoginUser(string login, string password);
    }
}