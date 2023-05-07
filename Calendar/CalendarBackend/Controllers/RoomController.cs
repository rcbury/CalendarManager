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

        public RoomController(IRoomRepository roomRepository, UserService userService, InviteLinkTokenGeneratorService inviteLinkTokenGeneratorService)
        {
            _roomRepository = roomRepository;
            _userService = userService;
            _inviteLinkTokenGeneratorService = inviteLinkTokenGeneratorService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRooms() 
        {
            var rooms = _roomRepository.GetAll();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = _roomRepository.GetById(id);
            return Ok(room);
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
            return Ok(inviteUrl);
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
        [Authorize]
        public async Task<IActionResult> ToggleAdmin(int id, int userId)
        {

            _roomRepository.ToggleAdmin(id, userId);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsRoomAdmin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            _roomRepository.DeleteById(id);
            return Ok();
        }


    }
}
