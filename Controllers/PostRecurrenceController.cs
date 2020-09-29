using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RAP.Contracts;
using RAP.DTOs;
using RAP.Persistence.Primary.Entities;

namespace RAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostRecurrenceController : ControllerBase
    {
        private readonly ILogger<PostRecurrenceController> _logger;
        private readonly IRedditService _redditService;
        private readonly PostRecurrenceEntity _postRecurrenceStorage;
        private readonly RedditUserEntity _redditUserStorage;
        public PostRecurrenceController(ILogger<PostRecurrenceController> logger, IRedditService redditService, IConfiguration config)
        {
            _logger = logger;
            _redditService = redditService;
            _postRecurrenceStorage = new PostRecurrenceEntity(config["ConnectionString"]);
            _redditUserStorage = new RedditUserEntity(config["ConnectionString"]);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestDto request)
        {
            var redditUser = await _redditUserStorage.FindAsync(request.RedditUserId);
            if (redditUser == null)
            {
                return BadRequest();
            }
            var postRecurrence = new PostRecurrenceEntity(request.Title, request.Body, string.Join(",", request.Subreddits), request.Interval, request.Email, request.RedditUserId);
            var savedPostRecrrence = await _postRecurrenceStorage.CreateAsync(postRecurrence);
            return Ok(new MessageDto($"Posts scheduled to begin {savedPostRecrrence.NextPost.UtcDateTime}. To stop recurring posts revoke access to app via reddit."));
        }

        [HttpPost("test")]
        public async Task<IActionResult> PostTest([FromBody] RequestDto request)
        {
            var redditUser = await _redditUserStorage.FindAsync(request.RedditUserId);
            if (redditUser == null)
            {
                return BadRequest();
            }
            var accessToken = await _redditService.RefreshIfExpiredAsync(redditUser.RefreshToken, redditUser.TokenExpiresAt, redditUser.AccessToken);
            if (accessToken != null)
            {
                // update record with refreshed token
                redditUser.RefreshToken = accessToken.RefreshToken;
                redditUser.TokenExpiresAt = accessToken.TokenExpiration;
                redditUser.Scope = accessToken.Scope;
                await redditUser.UpdateAsync(redditUser);
            }
            var post = await _redditService.CreatePostAsync("roybotsub", request.Title, request.Body);
            return Ok(new TestPostResponseDto(post.Url.ToString()));
        }
    }
}
