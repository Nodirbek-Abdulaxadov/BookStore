namespace BookStore.StorageService.Services;

public interface IS3Interface
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<string> UploadFileWithoutBGAsync(IFormFile file);
    Task<Stream> GetFileUrlAsync(string fileName);
    Task DeleteFileAsync(string fileName);
}