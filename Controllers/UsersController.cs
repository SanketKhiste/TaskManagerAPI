using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models.DTO;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUsers _users;

        public UsersController(IUsers users) 
        {
            _users = users;
        }

        [HttpGet("UsersDetails")]
        public async Task<ResponseDTO> UsersDetails()
        {
            var objRes = await _users.GetUserDetails();

            if (objRes == null || !objRes.IsSuccess)
            {
                return new ResponseDTO
                {
                    IsSuccess = false,
                    Message = objRes?.Message ?? "Failed to fetch user details.",
                    ResponseObject = null
                };
            }

            return objRes;
        }
    }
}
