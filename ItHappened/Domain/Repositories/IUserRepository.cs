namespace ItHappened.Domain.Repositories
{
    interface IUserRepository
    {
        Result<User> TryCreate(User user);
        Result<User> TryGet(string login, string hashedPassword);
    }
}