using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserServiceBL _userServiceBL;

        public UserController(ILogger<UserController> logger, IUserServiceBL userServiceBL)
        {
            _logger = logger;
            _userServiceBL = userServiceBL;
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userServiceBL.GetAllUsers();
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [HttpGet("getUserById/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userServiceBL.GetUserById(userId);


            return Ok(user);
        }

        [HttpGet("getUserByIdIncludeRole/{userId}")]
        public async Task<IActionResult> GetUserByIdIncludeRole(int userId)
        {
            var user = await _userServiceBL.GetUserByIdIncludeRole(userId);


            return Ok(user);
        }

        [HttpGet("getUserByNameIncludeRole/{firstName}/{lastName}")]
        public async Task<IActionResult> GetUserByNameIncludeRole(string firstName, string lastName)
        {
            var user = await _userServiceBL.GetUserByNameIncludeRole(firstName, lastName);

            return Ok(user);
        }

        [HttpGet("getUserByName/{firstName}/{lastName}")]
        public async Task<IActionResult> GetUserByName(string firstName, string lastName)
        {
            var user = await _userServiceBL.GetUserByName(firstName, lastName);
  

            return Ok(user);
        }

        [HttpGet("getUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userServiceBL.GetUserByEmail(email);


            return Ok(user);
        }

        [HttpGet("getUserByEmailAndPassword/{email}/{password}")]
        public async Task<IActionResult> GetUserByEmailAndPassword(string email, string password)
        {
            var user = await _userServiceBL.GetUserByEmailAndPassword(email, password);

            return Ok(user);
        }

        [HttpGet("getUserByIdAndPassword")]
        public async Task<IActionResult> GetUserByIdAndPassword([FromQuery]int userId, [FromQuery] string password)
        {
            var user = await _userServiceBL.GetUserByIdAndPassword(userId, password);

            return Ok(user);
        }

        [HttpGet("getUserByNameAndPassword/{firstName}/{lastName}/{password}")]
        public async Task<IActionResult> GetUserByNameAndPassword(string firstName, string lastName, string password)
        {
            var user = await _userServiceBL.GetUserByNameAndPassword(firstName, lastName, password);

            return Ok(user);
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser([FromBody] UserBL user)
        {
            await _userServiceBL.AddUser(user);
            return Ok($"User with id {user.UserId} added successfully.");
        }

        [HttpDelete("deleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userServiceBL.DeleteUser(userId);
            return Ok($"User with id {userId} deleted successfully.");
        }

        [HttpPatch("updateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] JsonPatchDocument<UserBL> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document cannot be null.");

            var updatedUser = await _userServiceBL.UpdateUser(userId, patchDoc);

            return Ok(updatedUser);
        }
    }
}
