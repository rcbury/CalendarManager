using CalendarBackend.Dto;
using System.Runtime.InteropServices;

namespace CalendarBackend.Repository.Interfaces
{
    public interface ICRUDRepository<TDto>
    {
        public List<TDto> GetAll();
        public TDto GetById(int id);
        public TDto Update(TDto dto);
        public TDto Create(TDto dto);
        public void DeleteById(int id);
    }
}
