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
    public class RoomController : Controller
    {
        private readonly IRoomRepository _roomRepository;
        private readonly UserService _userService;
        private readonly InviteLinkTokenGeneratorService _inviteLinkTokenGeneratorService;
        private readonly ITaskRepository _taskRepository;
		private readonly TaskService _taskService;
		private readonly IFileRepository _fileRepository;

        public RoomController(
				IRoomRepository roomRepository, 
				UserService userService, 
				InviteLinkTokenGeneratorService inviteLinkTokenGeneratorService, 
				ITaskRepository taskRepository,
				TaskService taskService,
				IFileRepository fileRepository)
        {
            _roomRepository = roomRepository;
            _userService = userService;
            _inviteLinkTokenGeneratorService = inviteLinkTokenGeneratorService;
            _taskRepository = taskRepository;
			_taskService = taskService;
			_fileRepository = fileRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserRooms()
        {
            var user = await _userService.GetUserByClaim(this.User);
            var rooms = _roomRepository.GetByUser(user.Id);
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = _roomRepository.GetById(id);
            return Ok(room);
        }


        [HttpGet("{id}/Users")]
        [Authorize(Policy = "IsRoomMember")]
        public async Task<IActionResult> GetUsers(int id)
        {
            var users = _roomRepository.GetUsersByRoom(id);

            return Ok(users);
        }

        [HttpGet("{id:int}/User/{userId:int}/Role")]
        [Authorize(Policy = "IsRoomMember")]
        public async Task<IActionResult> GetUserRole(int id, int userId)
        {
            var userRole = await _userService.GetUserRoleByRoom(userId, id);

            var userRoleDto = new UserRoleDto
            {
                Id = userRole.Id,
                Name = userRole.Name
            };

            return Ok(userRoleDto);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateRoom([FromBody] RoomDto room)
        {
            var user = await _userService.GetUserByClaim(this.User);
            var res = _roomRepository.Create(room, user);
            return Ok(res);
        }

        [HttpPut()]
        [Authorize(Policy = "IsRoomAdmin")]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomDto room)
        {
            var res = _roomRepository.Update(room);
            return Ok(res);
        }

        [HttpGet("{id}/inviteLink")]
        [Authorize(Policy = "IsRoomAdmin")]
        public async Task<IActionResult> GetInviteLink(int id)
        {
            var inviteToken = _inviteLinkTokenGeneratorService.GenerateRoomInviteToken(id);
            var inviteUrl = _inviteLinkTokenGeneratorService.GetInviteLink(inviteToken, id);

            var inviteLinkDto = new RoomInviteLinkDto { InviteLink = inviteUrl };
            return Ok(inviteLinkDto);
        }

        [HttpGet("{id}/acceptInvite")]
        [Authorize]
        public async Task<IActionResult> AddUserToRoomByLink(int id, string token)
        {

            var tokenIsValid = _inviteLinkTokenGeneratorService.CheckToken(token, id);

            if (tokenIsValid)
            {
                var user = await _userService.GetUserByClaim(this.User);
                _roomRepository.AddUser(id, user.Id);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}/ToggleAdmin")]
        [Authorize(Policy = "IsRoomAdmin")]
        public async Task<IActionResult> ToggleAdmin(int id, int userId)
        {
            _roomRepository.ToggleAdmin(id, userId);
            return Ok();
        }

        [HttpPost("{id}/KickUser")]
        [Authorize(Policy = "IsRoomAdmin")]
        public async Task<IActionResult> KickUser(int id, int userId)
        {
            _roomRepository.DeleteUser(id, userId);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsRoomCreator")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            _roomRepository.DeleteById(id);
            return Ok();
        }

		// TASKS 

        [HttpGet("{id}/tasks")]
        [Authorize("IsRoomMember")]
        public async Task<IActionResult> GetAllTasks(int id)
        {
            var tasks = _taskRepository.GetAll(id);
            return Ok(tasks);
        }

        [HttpGet("{id}/tasks/{taskId}")]
        [Authorize("IsRoomMember")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var task = _taskRepository.GetById(taskId);
            return Ok(task);
        }

        [HttpPost("{id}/tasks")]
        [Authorize("IsRoomMember")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto task)
        {
            var user = await _userService.GetUserByClaim(this.User);
            var res = _taskRepository.Create(task, user);
            return Ok(res);
        }

        [HttpPut("{id}/tasks/{taskId}")]
        [Authorize("IsRoomMember")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskDto task, [FromRoute] int taskId, [FromRoute] int id)
        {
			task.Id = taskId;
            var res = _taskRepository.Update(task);
            return Ok(res);
        }

        [HttpDelete("{id}/tasks/{taskId}")]
        [Authorize("IsRoomMember")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            _taskService.DeleteTask(taskId);
            return Ok();
        }

        [HttpPost("{id}/tasks/{taskId}/files")]
        [Authorize("IsRoomMember")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, int taskId)
        {
            var authorizedUser = this.User;

            var userIdClaim = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userIdClaim == null)
                return new BadRequestResult();

            var fileDto = await _taskService.UploadFile(taskId, file);

            if (fileDto != null)
            {
                return Ok(fileDto);
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpGet("{id}/tasks/{taskId}/files")]
        [Authorize("IsRoomMember")]
        public async Task<IActionResult> GetTaskFiles(int taskId)
        {
            var authorizedUser = this.User;

            var userIdClaim = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userIdClaim == null)
                return new BadRequestResult();

            var fileNames = _fileRepository.GetAll(taskId);

            if (fileNames != null)
            {
                return Ok(fileNames);
            }
            else
            {
                return new BadRequestResult();
            }
        }

        [HttpDelete("{id}/tasks/{taskId}/files/{fileId}")]
        [Authorize("IsRoomMember")]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            var authorizedUser = this.User;

            var userIdClaim = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userIdClaim == null)
                return new BadRequestResult();

            _taskService.DeleteFile(fileId);

            return new OkResult();
        }

        [HttpDelete("{id}/tasks/{taskId}/files")]
        [Authorize("IsRoomMember")]
        public async Task<IActionResult> DeleteTaskFiles(int taskId)
        {
            var authorizedUser = this.User;

            var userIdClaim = authorizedUser.Claims.Where(x => x.Type == "userId").FirstOrDefault();

            if (userIdClaim == null)
                return new BadRequestResult();

            _taskService.DeleteTaskFiles(taskId);

            return new OkResult();
        }


    }
}
