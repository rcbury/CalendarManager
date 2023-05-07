using CalendarBackend.Db;
using CalendarBackend.Dto;
using System.Runtime.InteropServices;

namespace CalendarBackend.Repository.Interfaces
{
    public interface IRoomRepository
    {
        public List<RoomDto> GetAll();
        public RoomDto GetById(int id);
        public RoomDto Update(RoomDto dto);
        public RoomDto Create(RoomDto dto, CalendarUser? user);
        public void DeleteById(int id);
    }
}
