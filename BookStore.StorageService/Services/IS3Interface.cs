namespace BookStore.StorageService.Services;

public interface IS3Interface
{
    public Task<string> UploadFileAsync(IFormFile file);
    public Task<Stream> GetFileUrlAsync(string fileName);
    public Task DeleteFileAsync(string fileName);
}