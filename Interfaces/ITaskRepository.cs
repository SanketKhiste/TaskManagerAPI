using TaskManagerAPI.Models;

namespace TaskManagerAPI.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItemResponse>> GetAllAsync();
        Task<IEnumerable<TaskItemResponse>> GetByIdAsync(int id);
        Task<int> CreateAsync(TaskItem task);
        Task<int> UpdateAsync(TaskItem task);
        Task<int> DeleteAsync(int id);
    }
}
