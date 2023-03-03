using System;
using System.Collections.Generic;

namespace CalendarBackend.Db;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<RoomUser> RoomUsers { get; } = new List<RoomUser>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
}
