using TinifyAPI;

namespace BookStore.StorageService.Services;

public class ExternalServices(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<byte[]> CopressImage(byte[] fileBytes)
    {
        Tinify.Key = _configuration["TinifyApiKey"];
        var compressedData = await Tinify.FromBuffer(fileBytes).ToBuffer();
        return compressedData;
    }

    public byte[] RemoveBackground(byte[] fileBytes)
    {
        using (var client = new HttpClient())
        using (var formData = new MultipartFormDataContent())
        {
            formData.Headers.Add("X-Api-Key", _configuration["RemoveBGKey"]);
            formData.Add(new ByteArrayContent(fileBytes), "image_file", "file.jpg");
            formData.Add(new StringContent("auto"), "size");
            var response = client.PostAsync("https://api.remove.bg/v1.0/removebg", formData).Result;

            if (response.IsSuccessStatusCode)
            {
                var stream = new FileStream("no-bg.png", FileMode.Create, FileAccess.Write, FileShare.None);
                var bytes = response.Content.ReadAsByteArrayAsync().Result;
                stream.Write(bytes, 0, bytes.Length);
                return bytes;
            }
            else
            {
                Console.WriteLine("Error: " + response.Content.ReadAsStringAsync().Result);
            }
        }

        return [];
    }
}