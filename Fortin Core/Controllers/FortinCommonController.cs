using Fortin.Common;
using Fortin.Common.Dtos;
using Fortin.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Fortin.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class FortinCommonController : ControllerBase
    {
        private readonly ILogger<FortinCommonController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserEFRepository _userEfRepository;

        public FortinCommonController(ILogger<FortinCommonController> logger, IUserRepository userRepository, IUserEFRepository userEFRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userEfRepository = userEFRepository;
        }

        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetById(id);

            return Ok(user);
        }

        [HttpGet("/users")]
        public async Task<ActionResult> GetAllUsers([FromQuery] UserResourceParameter userResourceParameter)
        {
            //var users = await _userRepository.GetUsers();
            var users = await _userEfRepository.GetUsersAsync(userResourceParameter);

            return Ok(users);
        }

        [HttpPost("/users")]
        public async Task<ActionResult> CreateUser([FromBody] AddUserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var createdUser = await _userEfRepository.AddUserAsync(user);

            return CreatedAtRoute("GetUserById", new { userId = createdUser.Id }, createdUser);
        }

        [HttpGet("/users/{userId}", Name = "GetUserById")]
        public async Task<ActionResult> GetUserByIdAsync(int userId)
        {
            var user = await _userEfRepository.GetUserById(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("/users/{userId}")]
        public async Task<IActionResult> UpdateUserAsync(long userId, [FromBody] UpdateUserDto user)
        {
            if (user == null || userId <= 0)
            {
                return BadRequest();
            }
            //if (existingUser == null)
            //{
            //    return NotFound();
            //}
            await _userEfRepository.UpdateUserAsync(userId, user);
            return NoContent();
        }

        [HttpDelete("/users/{userId}", Name ="DeleteUserById")]
        public async Task<IActionResult> DeleteUserAsync(long userId)
        {
            await _userEfRepository.DeleteUserAsync(userId);
            return NoContent();
        }
    }
}
