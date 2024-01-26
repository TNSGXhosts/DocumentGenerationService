using DocumentGenerateAPIService.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DocumentGenerateAPIService.DBModels;

public class FileRepository(AppDbContext dbContext) : IFileRepository
{
    public async Task AddFile(FileModel fileModel) {
        await dbContext.AddAsync(fileModel);
    }

    public async Task<List<FileModel>> GetFiles() {
        var files = await dbContext.Files.ToListAsync();
        return files;
    }

    public async Task<FileModel> GetFile(int fileId)
    {
        var fileModel = await dbContext.Files.FirstOrDefaultAsync(f => f.Id == fileId);

        if (fileModel == null)
            throw new Exception("File not found");

        return fileModel;
    }

    public async Task DeleteFile(int fileId)
    {
        var fileModel = await dbContext.Files.FirstOrDefaultAsync(f => f.Id == fileId);
        if (fileModel == null)
            throw new Exception("File not found");

        dbContext.Files.Remove(fileModel);
    }

    public async Task SaveChangesAsync() {
        await dbContext.SaveChangesAsync();
    }
}