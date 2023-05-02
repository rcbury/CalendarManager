public class ImageStorageService
{
    public ImageStorageService()
    {

    }

    public async Task<string> StoreProfileImageForUserOnDisk(int userId, IFormFile profilePicture)
    {
        var createdDirectory = System.IO.Directory.CreateDirectory($"StaticFiles/Images/{userId}/");
		//TODO: deal with picture extensions
		var filePath = createdDirectory.FullName + "profilePicture.jpg";

		using (Stream fileStream = new FileStream(filePath, FileMode.Create)){
			await profilePicture.CopyToAsync(fileStream);
		}

		return filePath;
    }
}
