using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TaskManagerAPI.Helpers;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class SignUpRepository : ISignUpRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SignUpRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> RegisterUserAsync(SignUpRequest signUp)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Name", signUp.Name);
                    parameters.Add("@Email", signUp.Email);

                    string encryptedPassword = EncryptionHelper.Encrypt(signUp.Password);
                    parameters.Add("@PasswordHash", encryptedPassword);

                    return await connection.ExecuteAsync("sp_RegisterUser", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database operation failed.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the user.", ex);
            }
        }
    }
}
