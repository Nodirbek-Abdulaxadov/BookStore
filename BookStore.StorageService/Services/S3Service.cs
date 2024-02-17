using Amazon.S3;
using Amazon.S3.Model;

namespace BookStore.StorageService.Services;

public class S3Service(IAmazonS3 s3Client)
    : IS3Interface
{
    private readonly IAmazonS3 s3Client = s3Client;
    private const string bucketName = "book-store-fn1";

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        using var fileStream = file.OpenReadStream();
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var fileKey = bucketName + "/" + fileName;

        var putObjectRequest = new PutObjectRequest
        {
            InputStream = fileStream,
            BucketName = bucketName,
            Key = fileName
        };

        await s3Client.PutObjectAsync(putObjectRequest);

        return fileName;
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
}