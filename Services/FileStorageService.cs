namespace EventManagementWebApp.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileStorageService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided for upload.");

            if (!file.ContentType.StartsWith("image/"))
                throw new ArgumentException("Only image files are allowed.");

            if (file.Length > 10 * 1024 * 1024)  
                throw new ArgumentException("File size exceeds the maximum allowed size (10MB).");

            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder); 
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folderPath, fileName); 
        }
    }
}
