using ItHappened.Domain.Repositories;

namespace ItHappened.Domain
{
    public interface IUserService
    {
        Result<AuthData> CreateUser(string login, string password);
        Result<AuthData> LoginUser(string login, string password);
    }
}