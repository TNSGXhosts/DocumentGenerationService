using Microsoft.AspNetCore.Mvc;
using Hangfire;
using DocumentGenerateAPIService.Models;
using DocumentGenerateAPIService.ConversionJobs;
using DocumentGenerateAPIService.FileService;
using Microsoft.AspNetCore.Cors;

[Route("api/files")]
[ApiController]
[EnableCors()]  
public class FileController(IFileService fileService, ILogger<FileController> logger) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] FileUploadModel model)
    {
        try
        {
            var file = Request.Form.Files[0];
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            var fileId = await fileService.AddFileAsync(model.File.FileName, model.File.OpenReadStream());

            BackgroundJob.Enqueue<ConversionJob>(x => x.ConvertHtmlToPdf(fileId, null));

            return Ok(new { Message = "File uploaded successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError("Error uploading file: " + ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("getFiles")]  
    public async Task<IActionResult> GetFiles()
    {
        var files = await fileService.GetFilesAsync();
        return Ok(files);
    }

    [HttpGet("download/{fileId}")]
    public async Task<IActionResult> DownloadFile(int fileId)
    {
        try
        {
            var fileInfo = await fileService.GetConvertedFileAsync(fileId);

            if (fileInfo == null)
            {
                logger.LogWarning($"File with ID {fileId} not found.");
                return NotFound();
            }

            var newFileName = Path.GetFileNameWithoutExtension(fileInfo.FileName) + ".pdf";

            return File(fileInfo.Content, "application/pdf", newFileName);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error downloading file with ID {fileId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("delete/{fileId}")]
    public async Task<IActionResult> DeleteFile(int fileId)
    {
        try
        {
            await fileService.DeleteFileAsync(fileId);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError($"Error deleting file with ID {fileId}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
