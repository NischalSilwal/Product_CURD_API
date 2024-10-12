namespace Product_CURD_API.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);
        void DeleteFileAsync(string FileNameWithExtension);
    }
}
