namespace CalendarBackend.Services;

public class FileStorageService
{
    public async Task<Tuple<string,string>> StoreFileForTaskOnDisk(int taskId, IFormFile file)
    {
        var createdDirectory = System.IO.Directory.CreateDirectory($"StaticFiles/TaskFiles/{taskId}/");
        var filePath = createdDirectory.FullName + file.FileName;

        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return new Tuple<string, string>(filePath, file.FileName);
    }

    public void DeleteFileFromDisk(string filePath)
    {
        File.Delete(filePath);
    }
}
