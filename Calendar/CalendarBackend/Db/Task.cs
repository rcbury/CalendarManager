using System;
using System.Collections.Generic;

namespace CalendarBackend.Db;

public partial class Task
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int RoomId { get; set; }

    public virtual ICollection<FileTask> FileTasks { get; } = new List<FileTask>();

    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<CalendarUser> Users { get; } = new List<CalendarUser>();
}
