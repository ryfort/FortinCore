using Fortin.Common;
using Fortin.Common.Dtos;
using Fortin.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Fortin.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("/login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            var response = await _loginService.LoginAsync(loginDto);

            return Ok(response);
        }
    }
}
