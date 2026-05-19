using KinderHub.Identity.DTOs;
using KinderHub.Identity.DTOs.Requests;
using KinderHub.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KinderHub.Identity.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto request)
        {
            var result = await _service.RegisterAsync(request);
            return Created(string.Empty, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request)
        {
            var result = await _service.LoginAsync(request);
            return Ok(result);
        }
    }
}