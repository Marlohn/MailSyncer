using MailSyncer.Application.Dtos;
using MailSyncer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MailSyncer.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServiceApplication _authenticationServiceApplication;

        public AuthenticationController(IAuthenticationServiceApplication authenticationServiceApplication)
        {
            _authenticationServiceApplication = authenticationServiceApplication;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var response = _authenticationServiceApplication.AuthenticateUser(request);

            if (response == null)
                return Unauthorized(new { Message = "Invalid credentials" });

            return Ok(response);
        }
    }
}