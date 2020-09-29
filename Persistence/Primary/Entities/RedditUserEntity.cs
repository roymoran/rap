using System;

namespace RAP.Persistence.Primary.Entities
{
    public class RedditUserEntity : BaseStorage<RedditUserEntity>, IBaseEntity
    {
        public string RedditUsername { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Scope { get; set; }
        public DateTimeOffset TokenExpiresAt { get; set; }

        public RedditUserEntity()
        {
        }

        public RedditUserEntity(string username, string accessToken, string refreshToken, string scope, DateTimeOffset tokenExpiration)
        {
            RedditUsername = username;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Scope = scope;
            TokenExpiresAt = tokenExpiration;
        }

        public RedditUserEntity(string connectionString) : base(connectionString)
        {
        }
    }
}
