using System;
using System.Collections.Generic;

namespace RAP.DTOs
{
    public class RequestDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> Subreddits { get; set; }
        public string Email { get; set; }
        public int Interval { get; set; }
        public Guid RedditUserId { get; set; }
        public RequestDto()
        {
        }
    }
}
