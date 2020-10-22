using ItHappened.Domain;

namespace ItHappend.RestAPI.Authentication
{
    public interface IJwtIssuer
    {
        string GenerateToken(UserDto user);
    }
}