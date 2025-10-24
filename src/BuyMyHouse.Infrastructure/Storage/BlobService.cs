using Azure.Storage.Blobs;

namespace BuyMyHouse.Infrastructure.Storage;

public class BlobService
{
    private readonly BlobServiceClient _client;
    private readonly string _containerName = "mortgage-docs";

    public BlobService(string connectionString)
    {
        _client = new BlobServiceClient(connectionString);
        var container = _client.GetBlobContainerClient(_containerName);
        container.CreateIfNotExists();
    }

    // public async Task<string> UploadFileAsync(string filePath, string fileName)
    // {
    //     var container = _client.GetBlobContainerClient(_containerName);
    //     var blob = container.GetBlobClient(fileName);
    //     await blob.UploadAsync(filePath, overwrite: true);
    //     return blob.Uri.ToString();
    // }

    public async Task<string> UploadFileAsync(string content, string fileName)
    {
        var container = _client.GetBlobContainerClient(_containerName);
        var blob = container.GetBlobClient(fileName);

        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        await blob.UploadAsync(stream, overwrite: true);

        return blob.Uri.ToString();
    }
}