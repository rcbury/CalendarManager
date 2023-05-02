using CalendarBackend.Dto;
using CalendarBackend.Repository.Interfaces;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(
            ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTasks(int roomId)
        {
            var tasks = _taskRepository.GetAll(roomId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = _taskRepository.GetById(id);
            return Ok(task);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto task)
        {
            var res = _taskRepository.Create(task);
            return Ok(res);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateTask([FromBody] TaskDto task)
        {
            var res = _taskRepository.Update(task);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            _taskRepository.DeleteById(id);
            return Ok();
        }
    }
}
