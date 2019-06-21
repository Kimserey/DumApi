using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DumApi.Controllers
{
    public class AccessToken
    {
        public string Token { get; set; }
    }

    public class Credentials
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public ActionResult<AccessToken> Post(Credentials credentials, [FromServices] IJwtToken jwtToken)
        {
            if (credentials.Username == "Kimserey")
            {
                return new AccessToken
                {
                    Token = jwtToken.Generate(credentials.Username)
                };
            }

            return Unauthorized();
        }
    }
}
