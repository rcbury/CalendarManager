using CalendarBackend.Dto;
using System.Runtime.InteropServices;

namespace CalendarBackend.Repository.Interfaces
{
    public interface IFileRepository
    {
        public List<FileDto> GetAll(int taskId);
        public FileDto GetById(int fileId);
        public void DeleteById(int id);
        FileDto Create(int taskId, FileDto fileDto);
    }
}
