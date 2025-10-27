using TaskManagerAPI.Models.DTO;

namespace TaskManagerAPI.Interfaces
{
    public interface IUsers
    {
        Task<ResponseDTO> GetUserDetails();
    }
}
