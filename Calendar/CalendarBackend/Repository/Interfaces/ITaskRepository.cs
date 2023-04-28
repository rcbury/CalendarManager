using CalendarBackend.Dto;
using System.Runtime.InteropServices;

namespace CalendarBackend.Repository.Interfaces
{
    public interface ITaskRepository
    {
        public List<TaskDto> GetAll(int roomId);
        public TaskDto GetById(int id);
        public TaskDto Update(TaskDto dto);
        public TaskDto Create(TaskDto dto);
        public void DeleteById(int id);
    }
}
