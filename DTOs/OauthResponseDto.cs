using System;
namespace RAP.DTOs
{
    public class OAuthResponseDto
    {
        public OAuthResponseDto(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }
}
