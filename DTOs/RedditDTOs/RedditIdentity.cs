using System;
using Newtonsoft.Json;

namespace RAP.DTOs.RedditDTOs
{
    public class RedditIdentity
    {
        public RedditIdentity(string name)
        {
            RedditUsername = name;
        }
        
        [JsonProperty("name")]
        public string RedditUsername;
    }
}
