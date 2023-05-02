using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CalendarBackend.Db;

public partial class CalendarUser : IdentityUser<int>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    public string? AvatarPath { get; set; }

    public virtual ICollection<RoomUser> RoomUsers { get; } = new List<RoomUser>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
    public virtual ICollection<Task> UsersTasks { get; } = new List<Task>();
}
