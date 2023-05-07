using CalendarBackend.Dto;
using CalendarBackend.Repository.Interfaces;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly TaskService _taskService;
        private readonly IFileRepository _fileRepository;
        private readonly UserService _userService;

        public TaskController(ITaskRepository taskRepository, TaskService taskService, 
            IFileRepository fileRepository, UserService userService)
        {
            _taskRepository = taskRepository;
            _taskService = taskService;
            _fileRepository = fileRepository;
            _userService = userService;
        }

        [HttpGet("all")]
        [Authorize("IsRoomAdmin")]
        public async Task<IActionResult> GetAllTasks(int roomId)
        {
            var tasks = _taskRepository.GetAll(roomId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        [Authorize("IsRoomAdmin")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = _taskRepository.GetById(id);
            return Ok(task);
        }

        [HttpPost()]
        [Authorize("IsRoomAdmin")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto task)
        {
            var user = await _userService.GetUserByClaim(this.User);
            var res = _taskRepository.Create(task, user);
            return Ok(res);
        }

        [HttpPut()]
        [Authorize("IsRoomAdmin")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskDto task)
        {
            var res = _taskRepository.Update(task);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        [Authorize("IsRoomAdmin")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            _taskRepository.DeleteById(id);
            return Ok();
        }

        [HttpPost("{id}/files")]
        [Authorize("IsRoomAdmin")]

        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, int id)
        {
            var authorizedUser = this.User;

            var userIdClaim = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userIdClaim == null)
                return new BadRequestResult();

            var fileDto = _taskService.UploadFile(id, file);

            if (fileDto != null)
            {
                return Ok(fileDto);
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpGet("{id}/files")]
        [Authorize("IsRoomAdmin")]
        public async Task<IActionResult> GetTaskFiles(int id)
        {
            var authorizedUser = this.User;

            var userIdClaim = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userIdClaim == null)
                return new BadRequestResult();

            var fileNames = _fileRepository.GetAll(id);

            if (fileNames != null)
            {
                return Ok(fileNames);
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpDelete("/files/{fileId}")]
        [Authorize("IsRoomAdmin")]
        public async Task<IActionResult> DeleteFile(int id, int fileId)
        {
            var authorizedUser = this.User;

            var userIdClaim = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userIdClaim == null)
                return new BadRequestResult();

            _taskService.DeleteFile(id);
            
            return new OkResult();
        }
    }
}
