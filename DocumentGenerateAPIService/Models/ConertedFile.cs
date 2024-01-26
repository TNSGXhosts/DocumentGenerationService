namespace DocumentGenerateAPIService.Models;

public class ConvertedFile
{
    public required string FileName { get;set; }
    public required byte[] Content { get;set; }
}