using System;
using System.Collections.Generic;

namespace CalendarBackend.Db;

public partial class FileTask
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public string FilePath { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
