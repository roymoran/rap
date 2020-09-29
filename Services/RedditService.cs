using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RAP.Contracts;
using RAP.DTOs.RedditDTOs;

namespace RAP.Services
{
    public class RedditService : IRedditService
    {
        private readonly Http _http = new Http();
        private readonly string _redirectUri;
        private readonly string _authEndpoint = "https://www.reddit.com/api/v1/access_token";
        private string _apiEndpoint = $"https://oauth.reddit.com/api";
        private readonly Dictionary<string, string> _bearerAuthHeader = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _basicAuthHeader = new Dictionary<string, string>();

        public RedditService(string appId, string appSecret, string redirectUri)
        {
            _redirectUri = redirectUri;
            string encodedCredentials = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes($"{appId}:{appSecret}"));
            _basicAuthHeader.Add("Authorization", $"Basic {encodedCredentials}");
            _bearerAuthHeader.Add("Authorization", $"Bearer ");
            _bearerAuthHeader.Add("User-Agent", $"netcore3.1:/r/ProgrammingPals:v0.0.1 (by /u/roybot93)");
        }

        public async Task<AccessTokenDto> RefreshIfExpiredAsync(string refreshToken, DateTimeOffset tokenExpiration, string currentToken)
        {
            AccessTokenDto accessToken = null;
            if (DateTimeOffset.Compare(tokenExpiration, DateTimeOffset.UtcNow) < 0)
            {
                accessToken = await Refresh(refreshToken);
                _bearerAuthHeader["Authorization"] = $"Bearer {accessToken.AccessToken}";
            }
            else
            {
                _bearerAuthHeader["Authorization"] = $"Bearer {currentToken}";
            }

            return accessToken;
        }

        public async Task<AccessTokenDto> Authorize(string code)
        {
            var accessTokenDto = await _http.Post<AccessTokenDto>($"{_authEndpoint}?grant_type=authorization_code&code={code}&redirect_uri={_redirectUri}", _basicAuthHeader);
            accessTokenDto.TokenExpiration = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + accessTokenDto.ExpiresIn);
            _bearerAuthHeader["Authorization"] = $"Bearer {accessTokenDto.AccessToken}";
            return accessTokenDto;
        }

        private async Task<AccessTokenDto> Refresh(string refreshToken)
        {
            var accessTokenDto = await _http.Post<AccessTokenDto>($"{_authEndpoint}?grant_type=refresh_token&refresh_token={refreshToken}", _basicAuthHeader);
            accessTokenDto.TokenExpiration = DateTimeOffset.FromUnixTimeSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + accessTokenDto.ExpiresIn);
            return accessTokenDto;
        }

        public async Task<PostData> CreatePostAsync(string subreddit, string title, string text)
        {
            var response = await _http.Post<RedditPostDataDto>($"{_apiEndpoint}/submit?api_type=json&sr={subreddit}&title={title}&text={Uri.EscapeDataString(text)}&kind=self", _bearerAuthHeader);
            return response.Json.Data;
        }

        public async Task<RedditIdentity> GetRedditIdentity()
        {
            var response = await _http.Get<RedditIdentity>($"{_apiEndpoint}/v1/me", _bearerAuthHeader);
            return response;
        }
    }
}
