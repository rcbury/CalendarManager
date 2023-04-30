using CalendarBackend.Db;
using CalendarBackend.Dto;
using CalendarBackend.Repository.Interfaces;
using Microsoft.AspNetCore.StaticFiles;

class RoomRepository : ICRUDRepository<RoomDto>
{
    private readonly CalendarDevContext _context;

    public RoomRepository(CalendarDevContext context)
    {
        _context = context;
    }

    public RoomDto Create(RoomDto room)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbRoom = new Room()
            {
                Name = room.Name
            };
            _context.Rooms.Add(dbRoom);
            _context.SaveChanges();
            transaction.Commit();
            var dto = new RoomDto { Id = dbRoom.Id, Name = dbRoom.Name };
            return dto;
        }
    }

    public void DeleteById(int id)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbRoom = _context.Rooms.Where(room => room.Id == id).FirstOrDefault();
            if (dbRoom != null) 
            {
                var dbRoomUsers = _context.RoomUsers.Where(ru => ru.RoomId == dbRoom.Id);
                _context.RoomUsers.RemoveRange(dbRoomUsers);
                _context.Rooms.Remove(dbRoom);
                _context.SaveChanges();
                transaction.Commit();
            }
        }
    }

    public List<RoomDto> GetAll()
    {
        var items = _context.Rooms
            .Select(item => new RoomDto { Id = item.Id, Name = item.Name })
            .ToList();
        return items;
    }

    public RoomDto GetById(int id)
    {
        var room = _context.Rooms
            .Where(item => item.Id == id)
            .Select(item => new RoomDto {Id = item.Id, Name = item.Name})
            .FirstOrDefault();
        room = room == null ? new RoomDto { Id = 0, Name = "Not found" } : room;
        return room;
    }

    public RoomDto Update(RoomDto room)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbRoom = _context.Rooms.Where(item => item.Id == room.Id).FirstOrDefault();
            if (dbRoom != null) 
            {
                dbRoom.Name = room.Name;
                _context.SaveChanges();
                transaction.Commit();
            }
            return room;
        }
    }
}
