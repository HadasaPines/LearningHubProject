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
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding the user. {ex.Message}");
            }
        }


    }
}
