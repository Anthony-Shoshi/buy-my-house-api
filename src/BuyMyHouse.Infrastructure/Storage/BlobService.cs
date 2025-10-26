using Azure.Storage.Blobs;
using Azure.Storage.Sas;

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

    public async Task<string> UploadFileAsync(string content, string fileName)
    {
        var container = _client.GetBlobContainerClient(_containerName);
        var blob = container.GetBlobClient(fileName);

        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        await blob.UploadAsync(stream, overwrite: true);

        return blob.Uri.ToString();
    }

    public async Task<string> UploadFileWithSasAsync(string content, string fileName, TimeSpan validFor)
    {
        var container = _client.GetBlobContainerClient(_containerName);
        await container.CreateIfNotExistsAsync();

        var blob = container.GetBlobClient(fileName);

        // Upload content
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        await blob.UploadAsync(stream, overwrite: true);

        // Generate SAS token
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerName,
            BlobName = fileName,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(validFor)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasUri = blob.GenerateSasUri(sasBuilder); // Requires client to have StorageSharedKeyCredential

        return sasUri.ToString();
    }

    public async Task<string> GetSasUrlAsync(string fileName, TimeSpan validFor)
    {
        var container = _client.GetBlobContainerClient(_containerName);
        var blob = container.GetBlobClient(fileName);

        if (!await blob.ExistsAsync())
        {
            throw new FileNotFoundException($"Blob '{fileName}' not found in container '{_containerName}'.");
        }

        // Generate SAS token
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerName,
            BlobName = fileName,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(validFor)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var sasUri = blob.GenerateSasUri(sasBuilder); // Requires client to have StorageSharedKeyCredential

        return sasUri.ToString();
    }
}