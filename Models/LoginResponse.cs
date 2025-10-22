namespace TaskManagerAPI.Models
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string? Token { get; set; }
    }
}
