using System;
using System.Collections.Generic;

namespace CalendarBackend.Db;

public partial class UserRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RoomUser> RoomUsers { get; } = new List<RoomUser>();
}
