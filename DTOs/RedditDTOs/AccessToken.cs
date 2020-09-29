using System;
using Newtonsoft.Json;

namespace RAP.DTOs.RedditDTOs
{
    public class AccessTokenDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        public DateTimeOffset TokenExpiration { get; set; }
    }
}
