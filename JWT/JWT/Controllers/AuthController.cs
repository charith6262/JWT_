using JWT.Model;
using JWT.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register(User user)
        {
            var userResult = await _authService.Register(user);
            if (userResult != "User registered successfully")
            {
                return BadRequest(new { message = userResult });
            }
            return Ok(new { message = userResult });
        }

        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var token = await _authService.Login(loginDTO);
            if (token != null)
            {
                return Unauthorized(new { message = "InvalidCredentials" });
            }
            return Ok(new { token });

        }
    }
}
