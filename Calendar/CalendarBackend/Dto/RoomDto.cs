using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto;

public class RoomDto
{
    public int? Id { get; set; }

    public string Name { get; set; } = null!;
}
