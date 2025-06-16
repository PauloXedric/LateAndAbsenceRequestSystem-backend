using Microsoft.AspNetCore.Hosting;

namespace DLARS.Services
{
    public interface IFileStorageService 
    {
      string SaveFile(IFormFile file, string folderName);
    }

    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string SaveFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0) return null;

            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return $"/{folderName}/{fileName}";
        }

    }
}
