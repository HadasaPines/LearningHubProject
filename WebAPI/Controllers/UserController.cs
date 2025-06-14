using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserManager _userManager;
        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        [HttpPut("adduser")]
        public async Task<IActionResult> AddUser([FromBody] UserBL userBL)
        {
            if (userBL == null)
            {
                return BadRequest("User data is required.");
            }
            try
            {
                await _userManager.AddUser(userBL);
                return Ok("User added successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("❌ SaveChangesAsync failed");
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("InnerException: " + ex.InnerException?.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("❌ SaveChangesAsync failed");
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("InnerException: " + ex.InnerException?.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex) { 
             Console.WriteLine("❌ SaveChangesAsync failed");
            Console.WriteLine("Message: " + ex.Message);
            Console.WriteLine("InnerException: " + ex.InnerException?.Message);
            
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding the user. {ex.Message}");
            }
        }
        [HttpGet("getuserbynameandpassword")]
        public async Task<IActionResult> GetUserByNameAndPassword(string firstName, string lastName, string password)
        {
            if (string.IsNullOrEmpty(firstName)|| string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Username and password are required.");
            }
            try
            {
                var user = await _userManager.GetUserByNameAndPassword(firstName, lastName, password);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the user. {ex.Message}");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int userId , string firstName, string lastName)
        {
            try
            {
                await _userManager.DeleteUser(firstName, lastName, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the user. {ex.Message}");
            }
        }
    }
}
