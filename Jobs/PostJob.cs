using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RAP.Contracts;
using RAP.Persistence.Primary.Entities;

namespace RAP.Jobs
{
    public class PostJob : BackgroundService
    {
        private readonly ILogger<PostJob> _logger;
        private readonly IRedditService _redditService;
        private readonly PostRecurrenceEntity _postRecurrenceStorage;
        private readonly RedditUserEntity _redditUserStorage;

        public PostJob(ILogger<PostJob> logger, IRedditService redditService, IConfiguration config)
        {
            _logger = logger;
            _postRecurrenceStorage = new PostRecurrenceEntity(config["ConnectionString"]);
            _redditUserStorage = new RedditUserEntity(config["ConnectionString"]);
            _redditService = redditService;
            _logger.LogInformation("Starting PostJob");
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var posts = await _postRecurrenceStorage.FindAllAsync();
                foreach (var post in posts)
                {
                    if (!ShouldStart(post.NextPost))
                    {
                        continue;
                    }

                    var redditUser = await _redditUserStorage.FindAsync(post.RedditUserId);
                    var accessToken = await _redditService.RefreshIfExpiredAsync(redditUser.RefreshToken, redditUser.TokenExpiresAt, redditUser.AccessToken);
                    if (accessToken != null)
                    {
                        // update record with refreshed token
                        redditUser.RefreshToken = accessToken.RefreshToken;
                        redditUser.TokenExpiresAt = accessToken.TokenExpiration;
                        redditUser.Scope = accessToken.Scope;
                        await redditUser.UpdateAsync(redditUser);
                    }

                    var subs = post.Subreddits.Split(",");

                    foreach (var sub in subs)
                    {
                        try
                        {
                            await _redditService.CreatePostAsync(sub, post.Title, post.Body);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogCritical($"Post failed for Post Id: {post.Id}, on Subreddit {sub}");
                            _logger.LogCritical(ex.Message, ex.StackTrace);
                        }
                    }

                    // reset next send
                    post.NextPost = DateTimeOffset.UtcNow.AddSeconds(post.IntervalSeconds);
                    await _postRecurrenceStorage.UpdateAsync(post);
                }

                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }

        public bool ShouldStart(DateTimeOffset? time)
        {
            if (!time.HasValue)
                return false;

            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            DateTimeOffset sendTime = time.Value;

            return currentTime.Year == sendTime.Year &&
                   currentTime.Month == sendTime.Month &&
                   currentTime.Day == sendTime.Day &&
                   currentTime.Hour == sendTime.Hour &&
                   currentTime.Minute >= sendTime.Minute &&
                   currentTime.Minute <= sendTime.Minute + 1;
        }
    }
}
