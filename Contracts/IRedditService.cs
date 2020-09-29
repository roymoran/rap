using System;
using System.Threading.Tasks;
using RAP.DTOs.RedditDTOs;

namespace RAP.Contracts
{
    public interface IRedditService
    {
        Task<PostData> CreatePostAsync(string subreddit, string title, string body);
        Task<AccessTokenDto> RefreshIfExpiredAsync(string refreshToken, DateTimeOffset tokenExpiration, string currentToken);
        Task<RedditIdentity> GetRedditIdentity();
        Task<AccessTokenDto> Authorize(string code);
    }
}
