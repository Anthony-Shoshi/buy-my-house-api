namespace BuyMyHouse.Domain.Repositories;

public interface IBlobService
    {
        Task<string> UploadFileAsync(string content, string fileName);
        Task<string> UploadFileWithSasAsync(string content, string fileName, TimeSpan validFor);
        Task<string> GetSasUrlAsync(string fileName, TimeSpan validFor);
    }
