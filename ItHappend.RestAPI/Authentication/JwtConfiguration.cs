using System;

namespace ItHappend.RestAPI.Authentication
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        
        public TimeSpan ExpiresAfter { get; set; }
    }
}