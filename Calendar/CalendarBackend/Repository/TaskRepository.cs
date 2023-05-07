using CalendarBackend.Db;
using CalendarBackend.Dto;
using CalendarBackend.Repository.Interfaces;
using System.Xml.Linq;

class TaskRepository : ITaskRepository 
{
    private readonly CalendarDevContext _context;

    public TaskRepository(CalendarDevContext context)
    {
        _context = context;
    }

    public TaskDto Create(TaskDto task, CalendarUser user)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbTask = new CalendarBackend.Db.Task()
            {
                Name = task.Name,
                Description = task.Description,
                RoomId = task.RoomId,
                DateStart = task.DateStart,
                DateEnd = task.DateEnd,
                IgnoreTime = task.IgnoreTime,
                CreatorId = user.Id
            };
            _context.Tasks.Add(dbTask);
            _context.SaveChanges();
            transaction.Commit();
            task.Id = dbTask.Id;
            return task;
        }
    }

    public void DeleteById(int id)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbTask = _context.Tasks.Where(task => task.Id == id).FirstOrDefault();
            if (dbTask != null)
            {
                _context.Tasks.Remove(dbTask);
                _context.SaveChanges();
                transaction.Commit();
            }
        }
    }

    public List<TaskDto> GetAll(int roomId)
    {
        var items = _context.Tasks
            .Where(item => item.RoomId == roomId)
            .Select(item => new TaskDto
            {
                Id = item.Id,
                CreatorId = item.CreatorId,
                DateStart = item.DateStart,
                DateEnd = item.DateEnd,
                Description = item.Description,
                IgnoreTime = item.IgnoreTime,
                Name = item.Name,
                RoomId = item.RoomId
            })
            .ToList();
        return items;
    }

    public TaskDto GetById(int id)
    {
        var task = _context.Tasks
            .Where(item => item.Id == id)
            .Select(item => new TaskDto
            {
                Id = item.Id,
                CreatorId = item.CreatorId,
                DateStart = item.DateStart,
                DateEnd = item.DateEnd,
                Description = item.Description,
                IgnoreTime = item.IgnoreTime,
                Name = item.Name,
                RoomId = item.RoomId
            })
            .FirstOrDefault();
        task = task == null ? new TaskDto { Id = 0, Name = "Not found" } : task;
        return task;
    }

    public TaskDto Update(TaskDto task)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbTask = _context.Tasks.Where(item => item.Id == task.Id).FirstOrDefault();
            if (dbTask != null)
            {
                dbTask.DateStart = task.DateStart;
                dbTask.DateEnd = task.DateEnd;
                dbTask.Description = task.Description;
                dbTask.IgnoreTime = task.IgnoreTime;
                dbTask.Name = task.Name;
                _context.SaveChanges();
                transaction.Commit();
            }
            return task;
        }
    }
}
