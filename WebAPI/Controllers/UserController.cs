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
        public async Task<ActionResult<List<UserBL>>> GetAllUsers()
        {
                var users = await _userServiceBL.GetAllUsers();
                return Ok(users);
        }
        [HttpGet("getUserById/{userId}")]
        public async Task<ActionResult<UserBL>> GetUserById(int userId)
        {
            var user = await _userServiceBL.GetUserById(userId);
            return Ok(user);
        }
        [HttpGet("getUserByIdIncludeRole/{userId}")]
        public async Task<ActionResult<UserBL>> GetUserByIdIncludeRole(int userId)
        {
            var user = await _userServiceBL.GetUserByIdIncludeRole(userId);
            return Ok(user);
        }
        [HttpGet("getUserByNameIncludeRole/{firstName}/{lastName}")]
        public async Task<ActionResult<UserBL>> GetUserByNameIncludeRole(string firstName, string lastName)
        {
            var user = await _userServiceBL.GetUserByNameIncludeRole(firstName, lastName);
            return Ok(user);
        }
        [HttpGet("getUserByName/{firstName}/{lastName}")]
        public async Task<ActionResult<UserBL>> GetUserByName(string firstName, string lastName)
        {
            var user = await _userServiceBL.GetUserByName(firstName, lastName);
            return Ok(user);
        }
        [HttpGet("getUserByEmail/{email}")]
        public async Task<ActionResult<UserBL>> GetUserByEmail(string email)
        {
            var user = await _userServiceBL.GetUserByEmail(email);
            return Ok(user);
        }
        [HttpGet("getUserByEmailAndPassword/{email}/{password}")]
        public async Task<ActionResult<UserBL>> GetUserByEmailAndPassword(string email, string password)
        {
            var user = await _userServiceBL.GetUserByEmailAndPassword(email, password);
            if (user == null)
            {
                return NotFound("User not found with the provided email and password.");
            }
            return Ok(user);

        }
        [HttpGet("getUserByIdAndPassword/{userId}/{password}")]
        public async Task<ActionResult<UserBL>> GetUserByIdAndPassword(int userId, string password)
        {
            var user = await _userServiceBL.GetUserByIdAndPassword(userId, password);
            if (user == null)
            {
                return NotFound("User not found with the provided ID and password.");
            }
            return Ok(user);
        }
        [HttpGet("getUserByNameAndPassword/{firstName}/{lastName}/{password}")]
        public async Task<ActionResult<UserBL>> GetUserByNameAndPassword(string firstName, string lastName, string password)
        {
            var user = await _userServiceBL.GetUserByNameAndPassword(firstName, lastName, password);
            if (user == null)
            {
                return NotFound("User not found with the provided name and password.");
            }
            return Ok(user);
        }
        [HttpPost("addUser")]
        public async Task<ActionResult> AddUser([FromBody] UserBL user)
        {

            await _userServiceBL.AddUser(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, user);
        }
        [HttpDelete("deleteUser/{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            await _userServiceBL.DeleteUser(userId);
            return NoContent();
        }
        [HttpPatch("updateUser/{userId}")]
        public async Task<ActionResult<UserBL>> UpdateUser(int userId, [FromBody] JsonPatchDocument<UserBL> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document cannot be null.");
            }

            var updatedUser = await _userServiceBL.UpdateUser(userId, patchDoc);

            return Ok(updatedUser);
        }





   
    }
}
