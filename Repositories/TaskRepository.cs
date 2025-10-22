using Dapper;
using System.Data;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Interfaces;
using System.Threading.Tasks;

namespace TaskManagerAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DapperContext _context;

        public TaskRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<TaskItem>(
                "sp_GetAllTasks", commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<TaskItem>> GetByIdAsync(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId, DbType.Int32);

            using var connection = _context.CreateConnection();
            var taskresult = await connection.QueryAsync<TaskItem>("sp_GetTaskById", parameters, commandType: CommandType.StoredProcedure);

            if (taskresult == null)
                throw new KeyNotFoundException($"Task with Id {userId} not found."); // Middleware will catch and return 404 if extended

            return taskresult;
        }

        public async Task<int> CreateAsync(TaskItem task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), "Task cannot be null"); // Middleware will return 500 with message

            var parameters = new DynamicParameters();
            parameters.Add("Title", task.Title);
            parameters.Add("Description", task.Description);
            parameters.Add("IsCompleted", task.IsCompleted);
            parameters.Add("DueDate", task.DueDate);
            parameters.Add("Priority", task.Priority);
            parameters.Add("UserId", task.UserId);

            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(
                "sp_InsertTask", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateAsync(TaskItem task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), "Task cannot be null");

            var parameters = new DynamicParameters();
            parameters.Add("Id", task.Id);
            parameters.Add("Title", task.Title);
            parameters.Add("Description", task.Description);
            parameters.Add("IsCompleted", task.IsCompleted);
            parameters.Add("DueDate", task.DueDate);
            parameters.Add("Priority", task.Priority);

            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(
                "sp_UpdateTask", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(
                "sp_DeleteTask", parameters, commandType: CommandType.StoredProcedure);

            if (rowsAffected == 0)
                throw new KeyNotFoundException($"Task with Id {id} not found."); // Middleware will return 404

            return rowsAffected;
        }
    }
}
