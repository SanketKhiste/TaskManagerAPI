using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.DTO;

namespace TaskManagerAPI.Repositories
{
    public class UsersRepository : IUsers
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly DapperContext _dapperContext;

        public UsersRepository(IConfiguration configuration, DapperContext dapperContext)
        {
            _configuration = configuration;
            _dapperContext = dapperContext;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ResponseDTO> GetUserDetails()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                var userList = (await connection.QueryAsync<Users>(
                    "sp_GetData",
                    commandType: CommandType.StoredProcedure
                )).ToList();

                return new ResponseDTO
                {
                    IsSuccess = true,
                    Message = "Data fetched successfully",
                    ResponseObject = userList
                };
            }
        }

    }
}
