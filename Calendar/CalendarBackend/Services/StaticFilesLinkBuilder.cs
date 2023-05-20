namespace CalendarBackend.Services;

public class StaticFilesLinkCreator
{

	ImageStorageService _imageStorageService;

    public StaticFilesLinkCreator(ImageStorageService imageStorageService)
    {
		_imageStorageService = imageStorageService;
    }

    public string? GetAvatarLink(int userId)
    {
		var avatarExists = _imageStorageService.CheckAvatarExists(userId);

		if (!avatarExists)
			return null;
		
		//probably move this to env variable
		var baseUrl = "https://localhost:7132";

		var serverPath = $"{baseUrl}/Static/Images/{userId}/profilePicture.jpg";

		return serverPath;
    }

    public string? GetFileLink(int taskId, string fileName)
    {

        //probably move this to env variable
        var baseUrl = "https://localhost:7132";

        var serverPath = $"{baseUrl}/Static/TaskFiles/{taskId}/{fileName}";

        return serverPath;
    }
}
