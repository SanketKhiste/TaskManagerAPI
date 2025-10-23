using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Helpers;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _login;

        public LoginController(ILoginRepository login) 
        {
            _login = login;
        }

        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin(LoginReq request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.PasswordHash))
                return BadRequest("Email and password are required.");
            string password = EncryptionHelper.Encrypt(request.PasswordHash);

            var response = await _login.LoginAsync(request.Email, password);

            if (response == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(response);
        }
    }
}
