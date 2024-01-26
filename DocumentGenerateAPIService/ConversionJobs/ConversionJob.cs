using DocumentGenerateAPIService.Common;
using DocumentGenerateAPIService.DBContext;
using Hangfire;
using Hangfire.Server;

namespace DocumentGenerateAPIService.ConversionJobs;

public class ConversionJob(ILogger<ConversionJob> logger, IFileRepository fileRepository) : BaseConversionJob
{
    [AutomaticRetry(Attempts = 3)] 
    public async Task ConvertHtmlToPdf(int fileId, PerformContext сontext)
    {
        int currentAttempt = сontext.GetJobParameter<int>("RetryCount");

        var fileModel = await fileRepository.GetFile(fileId);

        try
        {
            if (fileModel != null)
            {
                var pdfBytes = await ConvertHtmlToPdf(fileModel.Content);

                fileModel.PdfContent = pdfBytes;
                fileModel.Status = ConversionStatus.Success;

                await fileRepository.SaveChangesAsync();

                logger.LogInformation("File converted successfully");
            }
            else
            {
                logger.LogError("File not found");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error converting HTML to PDF");

            if (currentAttempt == 2)
            {
                fileModel.Status = ConversionStatus.Error;
                await fileRepository.SaveChangesAsync();
            }

            throw;
        }
    }
}
