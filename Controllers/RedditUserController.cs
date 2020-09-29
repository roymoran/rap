using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RAP.Contracts;
using RAP.DTOs.RedditDTOs;
using RAP.Persistence.Primary.Entities;

namespace RAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedditUserController : ControllerBase
    {
        private readonly ILogger<RedditUserController> _logger;
        private readonly RedditUserEntity _redditUserStorage;
        public RedditUserController(ILogger<RedditUserController> logger, IConfiguration config)
        {
            _logger = logger;
            _redditUserStorage = new RedditUserEntity(config["ConnectionString"]);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            var redditUser = await _redditUserStorage.FindAsync(Id);
            if (redditUser == null)
            {
                return BadRequest();
            }
            return Ok(new RedditIdentity(redditUser.RedditUsername));
        }
    }
}
