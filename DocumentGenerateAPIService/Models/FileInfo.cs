using DocumentGenerateAPIService.Common;

namespace DocumentGenerateAPIService.Models;

public class FileInfo
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public ConversionStatus Status { get; set; }
}