using System;

namespace Todo.Core.Entities
{
    public class TokenResponse
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpirationUtc { get; set; }
    }
}
