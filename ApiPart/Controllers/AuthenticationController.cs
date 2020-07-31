using ApiPart.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiPart.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }
        [HttpPost]
        public IActionResult Post([FromBody] User model)
        {
            var user = _authenticateService.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest(new { Message = "invalid username or password" });
            else
                return Ok(user);
        }
    }
}
