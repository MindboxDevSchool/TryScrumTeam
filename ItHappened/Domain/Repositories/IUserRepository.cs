using System;

namespace ItHappened.Domain.Repositories
{
    public interface IUserRepository
    {
        User TryCreate(User user);
        User TryGetByLogin(string login);
        User TryGetById(Guid id);
    }
}