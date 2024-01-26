using DocumentGenerateAPIService.DBModels;

namespace DocumentGenerateAPIService.DBContext;

public interface IFileRepository
{
    Task AddFile(FileModel fileModel);
    Task<List<FileModel>> GetFiles();
    Task<FileModel> GetFile(int fileId);
    Task DeleteFile(int fileId);
    Task SaveChangesAsync();
}