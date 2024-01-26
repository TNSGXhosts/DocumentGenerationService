using DocumentGenerateAPIService.Common;
using DocumentGenerateAPIService.DBContext;
using Hangfire;
using Hangfire.Server;

namespace DocumentGenerateAPIService.ConversionJobs;

public class RecurringConversionJob(ILogger<RecurringConversionJob> logger, IFileRepository fileRepository) : BaseConversionJob
{
    [AutomaticRetry(Attempts = 3)] 
    public async Task ConvertHtmlToPdf(PerformContext сontext)
    {
        int currentAttempt = сontext.GetJobParameter<int>("RetryCount");

        var files = await fileRepository.GetFiles();
        files = files.Where(f => f.Status == Common.ConversionStatus.InQueue).ToList();

        if (files.Any())
        {
            foreach (var file in files)
            {
                try
                {
                    var pdfBytes = await ConvertHtmlToPdf(file.Content);

                    file.PdfContent = pdfBytes;
                    file.Status = ConversionStatus.Success;

                    await fileRepository.SaveChangesAsync();

                    logger.LogInformation("File converted successfully");
                }
                catch(Exception ex)
                {
                    logger.LogError(ex, "Error converting HTML to PDF");

                    if (currentAttempt == 2)
                    {
                        file.Status = ConversionStatus.Error;
                        await fileRepository.SaveChangesAsync();
                    }

                    throw;
                }
            }
        }
    }
}