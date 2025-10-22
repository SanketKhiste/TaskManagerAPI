using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SignUpController : ControllerBase
    {
        private readonly ISignUpRepository _signUp;

        public SignUpController(ISignUpRepository sign)
        {
            _signUp = sign;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(SignUpRequest signup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int result = await _signUp.RegisterUserAsync(signup);

                if (result > 0)
                {
                    return Ok(new { message = "User registered successfully." });
                }
                else
                {
                    return BadRequest(new { message = "User registration failed." });
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email already exists"))
                    return Conflict(new { message = "Email already exists." });

                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
