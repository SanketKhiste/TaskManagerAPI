using TaskManagerAPI.Models;

namespace TaskManagerAPI.Interfaces
{
    public interface ISignUpRepository
    {
        Task<int> RegisterUserAsync(SignUpRequest signUp);
    }
}
