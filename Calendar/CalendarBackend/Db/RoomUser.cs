using System;
using System.Collections.Generic;

namespace CalendarBackend.Db;

public partial class RoomUser
{
    public int RoomId { get; set; }

    public int UserId { get; set; }

    public int UserRoleId { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual UserRole UserRole { get; set; } = null!;
}
