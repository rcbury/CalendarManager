using System.ComponentModel.DataAnnotations;

namespace CalendarBackend.Dto
{
    public class FileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
    }
}
