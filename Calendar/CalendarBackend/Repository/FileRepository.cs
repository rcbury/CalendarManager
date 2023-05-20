using CalendarBackend.Db;
using CalendarBackend.Dto;
using CalendarBackend.Repository.Interfaces;
using CalendarBackend.Services;
using System.Threading.Tasks;
using System.Xml.Linq;

class FileRepository : IFileRepository 
{
    private readonly CalendarDevContext _context;
    private readonly StaticFilesLinkCreator _staticFilesLinkCreator;

    public FileRepository(CalendarDevContext context, StaticFilesLinkCreator staticFilesLinkCreator)
    {
        _context = context;
        _staticFilesLinkCreator = staticFilesLinkCreator;
    }

    public FileDto Create(int taskId, FileDto fileDto)
    {
        using (var transaction = _context.Database.BeginTransaction()) 
        {
            var dbFile = new FileTask()
            {
                Name = fileDto.Name,
                FilePath = fileDto.Path,
                TaskId = taskId
            };
            _context.Add(dbFile);
            _context.SaveChanges();
            fileDto.Id = dbFile.Id;
            transaction.Commit();
            return fileDto;
        }
    }

    public void DeleteById(int id)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            var dbFile = _context.FileTasks.Where(item => item.Id == id).FirstOrDefault();
            if (dbFile != null)
            {
                _context.Remove(dbFile);
            }
            _context.SaveChanges();
            transaction.Commit();
        }
    }

    public List<FileDto> GetAll(int taskId)
    {
        var taskFiles = _context.FileTasks
            .Where(item => item.TaskId == taskId)
            .Select(item => new FileDto { Id = item.Id, Name = item.Name, Path = item.FilePath, Link = _staticFilesLinkCreator.GetFileLink(taskId, item.Name) })
            .ToList();
        return taskFiles;
    }

    public FileDto GetById(int fileId)
    {
        var fileDto = _context.FileTasks
            .Where(item => item.Id == fileId)
            .Select(item => new FileDto { Id = item.Id, Name = item.Name, Path = item.FilePath, Link = _staticFilesLinkCreator.GetFileLink(item.TaskId, item.Name) })
            .FirstOrDefault();
        
        if (fileDto == null) 
        {
            throw new Exception("File record not found");
        }
        
        return fileDto;
    }
}
