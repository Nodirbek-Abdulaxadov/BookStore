using Amazon.S3;
using Amazon.S3.Model;
using TinifyAPI;

namespace BookStore.StorageService.Services;

public class S3Service(IAmazonS3 s3Client,
                       IConfiguration configuration,
                       ExternalServices externalServices)
    : IS3Interface
{
    private readonly IAmazonS3 s3Client = s3Client;
    private readonly IConfiguration _configuration = configuration;
    private readonly ExternalServices _externalServices = externalServices;
    private const string bucketName = "book-store-fn1";

    public Task<string> UploadFileAsync(IFormFile file)
    {
        using var fileStream = file.OpenReadStream();
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var fileKey = bucketName + "/" + fileName;

        var fileBytes = new byte[fileStream.Length];
        fileStream.Read(fileBytes, 0, fileBytes.Length);

        _ = Task.Run(async () =>
        {
            var compressedData = await _externalServices.CopressImage(fileBytes);

            var putObjectRequest = new PutObjectRequest
            {
                InputStream = new MemoryStream(compressedData),
                BucketName = bucketName,
                Key = fileName
            };

            await s3Client.PutObjectAsync(putObjectRequest);
        });

        return Task.FromResult(fileName);
    }

    public async Task<Stream> GetFileUrlAsync(string fileName)
    {
        var urlRequest = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = fileName,
            Expires = DateTime.Now.AddMinutes(5)
        };

        var fileUrl = s3Client.GetPreSignedURL(urlRequest);
        HttpClient httpClient = new();

        var response = await httpClient.GetAsync(fileUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new ArgumentNullException("File not found");
        }

        return response.Content.ReadAsStream();
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = fileName
        };

        await s3Client.DeleteObjectAsync(deleteObjectRequest);
    }

    public Task<string> UploadFileWithoutBGAsync(IFormFile file)
    {
        using var fileStream = file.OpenReadStream();
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var fileKey = bucketName + "/" + fileName;

        var fileBytes = new byte[fileStream.Length];
        fileStream.Read(fileBytes, 0, fileBytes.Length);

        _ = Task.Run(async () =>
        {
            var compressedData = await _externalServices.CopressImage(fileBytes);
            var bytes = _externalServices.RemoveBackground(compressedData);

            var putObjectRequest = new PutObjectRequest
            {
                InputStream = new MemoryStream(bytes),
                BucketName = bucketName,
                Key = fileName
            };

            await s3Client.PutObjectAsync(putObjectRequest);
        });

        return Task.FromResult(fileName);
    }
}