using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public LoginRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            LoginResponse objResponse = new LoginResponse();
            using (var connection = new SqlConnection(_connectionString))
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("Email", email, direction: ParameterDirection.Input, dbType: DbType.String);
                dynamicParameters.Add("PasswordHash", password, direction: ParameterDirection.Input, dbType: DbType.String);
                dynamicParameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                objResponse = connection.Query<LoginResponse>("sp_LoginUser", param: dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();;

                var result = dynamicParameters.Get<int>("Result");

                if (result == 1)
                {
                    if (objResponse.Email == email)
                    {
                        var claims = new List<Claim>
                            {
                                new Claim("Id", objResponse.UserId.ToString()),
                                new Claim("Name", objResponse.Name),
                                new Claim("Email", objResponse.Email),
                                new Claim("RoleName", objResponse.RoleName)
                            };

                        claims.Add(new Claim(ClaimTypes.Role, objResponse.RoleName));

                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var tokeOptions = new JwtSecurityToken(
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            claims: claims,
                            expires: DateTime.Now.AddSeconds(15),
                            signingCredentials: signinCredentials
                            );
                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                        objResponse.Token = tokenString;
                    }
                }
                return objResponse;
            }
        }
    }
}
