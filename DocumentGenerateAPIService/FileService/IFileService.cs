using DocumentGenerateAPIService.DBModels;
using DocumentGenerateAPIService.Models;

namespace DocumentGenerateAPIService.FileService;

public interface IFileService
{
    Task<int> AddFileAsync(string fileName, Stream fileStream);
    Task<Models.FileInfo> GetFileAsync(int id);
    Task<IEnumerable<Models.FileInfo>> GetFilesAsync();
    Task<ConvertedFile> GetConvertedFileAsync(int id);
    Task DeleteFileAsync(int id);
}