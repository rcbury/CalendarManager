using CalendarBackend.Dto;
using CalendarBackend.Repository.Interfaces;

namespace CalendarBackend.Services;

public class TaskService
{
    private readonly IFileRepository _fileRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly FileStorageService _fileStorageService;

    public TaskService(IFileRepository fileRepository, ITaskRepository taskRepository, FileStorageService fileStorageService)
    {
        _fileRepository = fileRepository;
        _fileStorageService = fileStorageService;
        _taskRepository = taskRepository;
    }

    public async Task<FileDto> UploadFile(int taskId, IFormFile file)
    {
        var fileTuple = await _fileStorageService.StoreFileForTaskOnDisk(taskId, file);

        var fileDto = new FileDto
        {
            Name = fileTuple.Item2,
            Path = fileTuple.Item1
        };

        fileDto = _fileRepository.Create(taskId, fileDto);

        return fileDto;
    }

    public async void DeleteFile(int id)
    {
        var fileDto = _fileRepository.GetById(id);

        _fileStorageService.DeleteFileFromDisk(fileDto.Path);

        _fileRepository.DeleteById(id);
    }

    public async void DeleteTask(int id)
    {
        var taskDto = _taskRepository.GetById(id);

        if (taskDto != null) 
        {
            var files = _fileRepository.GetAll(taskDto.Id ?? 0);

            foreach (var file in files) 
            {
                DeleteFile(file.Id);
                _fileRepository.DeleteById(id);
                _taskRepository.DeleteById(id);
            }
        }
    }
}
