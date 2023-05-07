using System;
using System.Collections.Generic;

namespace CalendarBackend.Db;

public partial class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public int AuthorId { get; set; }

    public virtual ICollection<RoomUser> RoomUsers { get; } = new List<RoomUser>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
