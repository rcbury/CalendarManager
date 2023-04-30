using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int RoomId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool IgnoreTime { get; set; }
        public int CreatorId { get; set; }
        public IFormFileCollection Files { get; set; }
        public List<UserDto> Users { get; set; }
    }
}
