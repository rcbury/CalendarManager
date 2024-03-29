using CalendarBackend.Db;
using CalendarBackend.Dto;
using CalendarBackend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using CalendarBackend.Services;

class RoomRepository : IRoomRepository
{
    private readonly CalendarDevContext _context;
    private readonly StaticFilesLinkCreator _staticFilesLinkCreator;

    public RoomRepository(CalendarDevContext context, StaticFilesLinkCreator staticFilesLinkCreator)
    {
        _context = context;
        _staticFilesLinkCreator = staticFilesLinkCreator;
    }

    public void AddUser(int roomId, int userId)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbRoom = _context.Rooms.Where(item => item.Id == roomId).FirstOrDefault();
            if (dbRoom != null)
            {
                var dbRoomUsers = new RoomUser
                {
                    RoomId = dbRoom.Id,
                    UserId = userId,
                    UserRoleId = 2
                };
                _context.Add(dbRoomUsers);
                _context.SaveChanges();
            }
            transaction.Commit();
        }
    }

    public RoomDto Create(RoomDto room, CalendarUser? user)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbRoom = new Room()
            {
                Name = room.Name,
                AuthorId = user.Id
            };
            _context.Add(dbRoom);
            _context.SaveChanges();
            var dbRoomUsers = new RoomUser
            {
                RoomId = dbRoom.Id,
                UserId = user.Id,
                UserRoleId = 1
            };
            _context.Add(dbRoomUsers);
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
                _context.SaveChanges();

				var dbRoomFileTasks = _context.FileTasks
					.Include(taskFile => taskFile.Task)
					.Where(taskFile => taskFile.Task.RoomId == dbRoom.Id);

                _context.FileTasks.RemoveRange(dbRoomFileTasks);
                _context.SaveChanges();

                var dbRoomTasks = _context.Tasks.Include(task => task.Users).Where(task => task.RoomId == dbRoom.Id);
				foreach (var dbRoomTask in dbRoomTasks)
				{
					dbRoomTask.Users.Clear();
				}
                _context.SaveChanges();
                _context.Tasks.RemoveRange(dbRoomTasks);


                _context.Rooms.Remove(dbRoom);
                _context.SaveChanges();
                transaction.Commit();
            }
        }
    }

    public void DeleteUser(int roomId, int userId)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbRoom = _context.Rooms.Where(room => room.Id == roomId).FirstOrDefault();
            if (dbRoom != null)
            {
                var dbRoomUsers = _context.RoomUsers
					.Where(ru => ru.RoomId == dbRoom.Id && ru.UserId == userId && ru.UserRoleId != 1).ToList();


                var dbTasks = _context.Tasks.Where(item => item.RoomId == roomId).Include(x => x.Users).ToList();
                foreach (var dbTask in dbTasks) 
                {
                    dbTask.Users.Remove(dbRoomUsers.First().User);
                }
                _context.RoomUsers.RemoveRange(dbRoomUsers);
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
            .Select(item => new RoomDto { Id = item.Id, Name = item.Name, AuthorId = item.AuthorId })
            .FirstOrDefault();
        room = room == null ? new RoomDto { Id = 0, Name = "Not found" } : room;
        return room;
    }

    public List<RoomDto> GetByUser(int userId)
    {
        var rooms = _context.Rooms
            .Include(room => room.RoomUsers)
            .Where(item => item.RoomUsers.Any(x => x.UserId == userId))
            .Select(item => new RoomDto { Id = item.Id, Name = item.Name, AuthorId = item.AuthorId })
            .ToList();

        return rooms;
    }

    public List<UserRoomDto> GetUsersByRoom(int roomId)
    {
        var users = _context.RoomUsers
            .Include(roomUser => roomUser.User)
            .Where(roomUser => roomUser.RoomId == roomId)
            .Select(roomUser => new UserRoomDto
            {
                Id = roomUser.User.Id,
                UserName = roomUser.User.UserName,
                Email = roomUser.User.Email,
                FirstName = roomUser.User.FirstName,
                LastName = roomUser.User.LastName,
                UserRoleId = roomUser.UserRoleId,
                AvatarPath = _staticFilesLinkCreator.GetAvatarLink(roomUser.User.Id)
            })
            .ToList();

        return users;
    }

    public bool ToggleAdmin(int roomId, int userId)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbRoom = _context.Rooms.Where(item => item.Id == roomId).FirstOrDefault();
            if (dbRoom != null)
            {
                var roomUsers = _context.RoomUsers.Where(item => item.RoomId == dbRoom.Id && item.UserId == userId).FirstOrDefault();
                if (roomUsers != null)
                {
                    if (roomUsers.UserRoleId == 1)
                    {
                        roomUsers.UserRoleId = 2;
                        _context.SaveChanges();
                        transaction.Commit();
                        return false;
                    }
                    else if (roomUsers.UserRoleId == 2)
                    {
                        roomUsers.UserRoleId = 1;
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                }
            }
        }
        return false;
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
