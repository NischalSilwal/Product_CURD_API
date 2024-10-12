
using Microsoft.AspNetCore.Hosting;
using System.Xml.XPath;

namespace Product_CURD_API.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void DeleteFileAsync(string FileNameWithExtension)
        {
            if (string.IsNullOrEmpty(FileNameWithExtension))
            {
                throw new ArgumentNullException(nameof(FileNameWithExtension));
            }
            var contentpath = _webHostEnvironment.ContentRootPath;
            var path = Path.Combine(contentpath, $"Uploads", FileNameWithExtension);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Invalid File path");
            } 
            File.Delete(path);
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }
            var contentPath = _webHostEnvironment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(path)) { 
             Directory.CreateDirectory(path);
            }

            //check if the extension is allowed or not

            var ext = Path.GetExtension(imageFile.FileName);
            if (!allowedFileExtensions.Contains(ext.ToLower()))
            {
                throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
            }


            //Generate a unique filename
            var fileName = $"{Guid.NewGuid().ToString()} {ext}";
            var fileNameWithPath = Path.Combine(path, fileName);

            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);
            return fileName;
        }
    }
}
