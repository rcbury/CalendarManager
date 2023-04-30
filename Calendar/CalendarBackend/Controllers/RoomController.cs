using CalendarBackend.Dto;
using CalendarBackend.Repository.Interfaces;
using CalendarBackend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : Controller
    {
        private readonly ICRUDRepository<RoomDto> _roomRepository;

        public RoomController(
            ICRUDRepository<RoomDto> roomRepository)
        {
            _roomRepository = roomRepository;
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
        public async Task<IActionResult> CreateRoom([FromBody] RoomDto room)
        {
            var res = _roomRepository.Create(room);
            return Ok(res);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateRoom([FromBody] RoomDto room)
        {
            var res = _roomRepository.Update(room);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            _roomRepository.DeleteById(id);
            return Ok();
        }


    }
}
