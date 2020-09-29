using System;
namespace RAP.DTOs
{
    public class TestPostResponseDto
    {
        public string PostUrl { get; set; }
        public TestPostResponseDto(string postUrl)
        {
            PostUrl = postUrl;
        }
    }
}
