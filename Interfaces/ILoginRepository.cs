using TaskManagerAPI.Models;

namespace TaskManagerAPI.Interfaces
{
    public interface ILoginRepository
    {
        Task<LoginResponse> LoginAsync(string email, string password);
    }
}
