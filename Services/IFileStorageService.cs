namespace EventManagementWebApp.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderPath);
    }
}
