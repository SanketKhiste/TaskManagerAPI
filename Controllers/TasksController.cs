using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using TaskManagerAPI.Interfaces;
using System.Threading.Tasks;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _repository;

        public TasksController(ITaskRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _repository.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("GetTaskById/{userId}")]
        public async Task<IActionResult> GetTaskById(int userId)
        {
            var result = await _repository.GetByIdAsync(userId);
            if (result == null)
                return NotFound(new { message = $"Task with Id {userId} not found." });

            return Ok(result);
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(TaskItem task)
        {
            if (task == null)
                return BadRequest(new { message = "Invalid task data." });

            await _repository.CreateAsync(task);
            return Ok(new { message = "Task created successfully." });
        }

        [HttpPut("UpdateTask")]
        public async Task<IActionResult> UpdateTask(TaskItem task)
        {
            //if (task == null)
            //    return BadRequest(new { message = "Invalid task data." });

            //var existingTask = await _repository.GetByIdAsync(task.Id);
            //if (existingTask == null)
            //    return NotFound(new { message = $"Task with Id {task.Id} not found." });

            //task.Id = id; // ensure correct ID
            await _repository.UpdateAsync(task);
            return Ok(new { message = "Task updated successfully." });
        }

        [HttpDelete("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var existingTask = await _repository.DeleteAsync(id);
            if (existingTask == null)
                return NotFound(new { message = $"Task with Id {id} not found." });

            return Ok(new { message = "Task deleted successfully." });
        }
    }
}
