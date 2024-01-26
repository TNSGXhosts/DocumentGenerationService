using System.ComponentModel.DataAnnotations;
using DocumentGenerateAPIService.Common;

namespace DocumentGenerateAPIService.DBModels;

public class FileModel
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public required string Content { get; set; }
    public ConversionStatus Status { get; set; }
    public byte[]? PdfContent { get; set; }
}