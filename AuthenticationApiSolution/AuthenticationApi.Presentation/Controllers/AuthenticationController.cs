using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IUser usreInterface) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AppUserDTO appUserDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await usreInterface.Register(appUserDTO);
            
              
            return response.Flag?Ok(response) : BadRequest(Request);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await usreInterface.Login(loginDTO);
            return response.Flag ? Ok(response) : BadRequest(Request);
        }
        [HttpGet("GetUser/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int userId)
        {
            if (userId <= 0) return BadRequest("Invalid user Id");
            var response = await usreInterface.GetUser(userId);
            return response == null ? NotFound() : Ok(response);
        }
    }
}
