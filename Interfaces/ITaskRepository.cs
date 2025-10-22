using TaskManagerAPI.Models;

namespace TaskManagerAPI.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<IEnumerable<TaskItem>> GetByIdAsync(int id);
        Task<int> CreateAsync(TaskItem task);
        Task<int> UpdateAsync(TaskItem task);
        Task<int> DeleteAsync(int id);
    }
}
