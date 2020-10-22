namespace ItHappend.RestAPI.Models
{
    public class LoginResponse
    {
        public LoginResponse(string accessToken)
        {
            AccessToken = accessToken;
        }

        public string AccessToken { get; }
    }
}