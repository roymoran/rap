using System;
using Newtonsoft.Json;

namespace RAP.DTOs
{
    public class OAuthDto
    {
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }

    }
}
