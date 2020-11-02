using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAP.Persistence.Primary.Entities
{
    public class PostRecurrenceEntity : BaseStorage<PostRecurrenceEntity>, IBaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Subreddits { get; set; }
        public ulong IntervalSeconds { get; set; }
        public string Email { get; set; }
        [ForeignKey("RedditUserId")]
        public RedditUserEntity RedditUser { get; set; }
        public Guid RedditUserId { get; set; }
        public DateTimeOffset LastPost { get; set; }
        public DateTimeOffset NextPost { get; set; }

        public PostRecurrenceEntity()
        {
        }

        public PostRecurrenceEntity(string title, string body, string subreddits, int intervalSeconds, string email, Guid redditUserId)
        {
            Title = title;
            Body = body;
            Subreddits = subreddits;
            Email = email;
            RedditUserId = redditUserId;
            IntervalSeconds = MapValueToSeconds(intervalSeconds);
            NextPost = DateTimeOffset.UtcNow.AddMinutes(3);
        }

        public PostRecurrenceEntity(string connectionString) : base(connectionString)
        {
        }

        private ulong MapValueToSeconds(int value)
        {
            return value switch
            {
                0 => 2592000,
                1 => 1209600,
                2 => 604800,
                _ => 2592000,
            };
        }
    }
}
