

namespace DLARS.Services
{
    public interface IFileStorageService 
    {
      string SaveFile(IFormFile file, string folderName);
    }

    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(IWebHostEnvironment webHostEnvironment, ILogger<FileStorageService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public string SaveFile(IFormFile file, string folderName)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("Empty or null file provided.");
                    return string.Empty;
                }

                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);

                return $"/{folderName}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving a file to folder {}", folderName);
                throw;
            }
        }

    }
}
