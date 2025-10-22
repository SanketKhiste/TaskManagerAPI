using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class SignUpRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
