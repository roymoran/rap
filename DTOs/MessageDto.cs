using System;
using System.Collections.Generic;

namespace RAP.DTOs
{
    public class MessageDto
    {
        public string Message { get; set; }
        public MessageDto(string message)
        {
            Message = message;
        }
    }
}
