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

        public RoomController(IRoomRepository roomRepository, UserService userService)
        {
            _roomRepository = roomRepository;
            _userService = userService;
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

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsRoomAdmin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            _roomRepository.DeleteById(id);
            return Ok();
        }


    }
}
