using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RAP.Contracts;
using RAP.DTOs;
using RAP.Persistence.Primary.Entities;

namespace RAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly ILogger<OAuthController> _logger;
        private readonly IRedditService _redditService;
        private readonly RedditUserEntity _redditUserStorage;
        public OAuthController(ILogger<OAuthController> logger, IRedditService redditService, IConfiguration config)
        {
            _logger = logger;
            _redditService = redditService;
            _redditUserStorage = new RedditUserEntity(config["ConnectionString"]);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OAuthDto oauth)
        {
            var accessToken = await _redditService.Authorize(oauth.Code);
            var redditIdentity = await _redditService.GetRedditIdentity();
            var redditUser = await _redditUserStorage.FindByAsync(r => r.RedditUsername == redditIdentity.RedditUsername);
            if (redditUser == null)
            {
                var redditEntity = new RedditUserEntity(redditIdentity.RedditUsername, accessToken.AccessToken, accessToken.RefreshToken, accessToken.Scope, accessToken.TokenExpiration);
                var savedRedditUserEntity = await _redditUserStorage.CreateAsync(redditEntity);
                return Ok(new OAuthResponseDto(savedRedditUserEntity.Id));
            }
            else
            {
                // return stored user but update access token
                redditUser.AccessToken = accessToken.AccessToken;
                redditUser.RefreshToken = accessToken.RefreshToken;
                redditUser.Scope = accessToken.Scope;
                redditUser.TokenExpiresAt = accessToken.TokenExpiration;
                await _redditUserStorage.UpdateAsync(redditUser);
                return Ok(new OAuthResponseDto(redditUser.Id));
            }
        }
    }
}
