using System;
using Newtonsoft.Json;

namespace RAP.DTOs.RedditDTOs
{
    public class RedditPostDataDto
    {
        [JsonProperty("json")]
        public Json Json { get; set; }
    }

    public class Json
    {
        [JsonProperty("errors")]
        public object[] Errors { get; set; }

        [JsonProperty("data")]
        public PostData Data { get; set; }
    }

    public partial class PostData
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("drafts_count")]
        public long DraftsCount { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
