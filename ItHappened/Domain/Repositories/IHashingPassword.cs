namespace ItHappened.Domain.Repositories
{
    public interface IHashingPassword
    {
        string HashPassword(string password);
        bool Verify(string password, string hashedPassword);
    }
}