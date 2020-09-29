using System;
namespace RAP
{
    public class Settings
    {
        public string RedditAppId { get; set; } = Environment.GetEnvironmentVariable("RedditAppId", EnvironmentVariableTarget.Process);
        public string RedditAppSecret { get; set; } = Environment.GetEnvironmentVariable("RedditAppSecret", EnvironmentVariableTarget.Process);
        public string RedditRedirectUri { get; set; } = Environment.GetEnvironmentVariable("RedditRedirectUri", EnvironmentVariableTarget.Process);
        public string ConnectionString { get; set; } = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process);
    }
}
