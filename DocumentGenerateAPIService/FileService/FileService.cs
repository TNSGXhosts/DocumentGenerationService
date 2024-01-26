using AutoMapper;
using DocumentGenerateAPIService.Common;
using DocumentGenerateAPIService.DBContext;
using DocumentGenerateAPIService.DBModels;
using DocumentGenerateAPIService.Models;

namespace DocumentGenerateAPIService.FileService;

public class FileService(IMapper mapper, IFileRepository fileRepository) : IFileService
{
    public async Task<int> AddFileAsync(string fileName, Stream fileStream)
    {
        if (fileStream == null || fileStream.Length == 0)
        {
            throw new Exception("Invalid file");
        }

        var fileId = 0;
        using (var streamReader = new StreamReader(fileStream))
        {
            var fileModel = new FileModel
            {
                FileName = fileName,
                Content = await streamReader.ReadToEndAsync(),
                Status = ConversionStatus.InQueue
            };

            await fileRepository.AddFile(fileModel);
            await fileRepository.SaveChangesAsync();

            fileId = fileModel.Id;
        }

        return fileId;
    }

    public async Task<Models.FileInfo> GetFileAsync(int id)
    {
        var fileModel = await fileRepository.GetFile(id);
        var fileInfo = mapper.Map<Models.FileInfo>(fileModel);
        return fileInfo;
    }

    public async Task<IEnumerable<Models.FileInfo>> GetFilesAsync()
    {
        var files = await fileRepository.GetFiles();
        var fileInfos = files.Select(f => mapper.Map<Models.FileInfo>(f));
        return fileInfos;
    }

    public async Task<ConvertedFile> GetConvertedFileAsync(int id)
    {
        var file = await fileRepository.GetFile(id);
        if (file == null || file.PdfContent == null)
        {
            throw new Exception("File not found");
        }

        var convertedFile = mapper.Map<ConvertedFile>(file);

        return convertedFile;
    }

    public async Task DeleteFileAsync(int id)
    {
        await fileRepository.DeleteFile(id);
        await fileRepository.SaveChangesAsync();
    }
}
