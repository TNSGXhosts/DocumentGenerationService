using PuppeteerSharp;

namespace DocumentGenerateAPIService.ConversionJobs;

public abstract class BaseConversionJob
{
    public async Task<byte[]> ConvertHtmlToPdf(string htmlContent)
    {
        try
        {
            var launchOptions = new LaunchOptions
            {
                Headless = true, // Run without UI
                //ExecutablePath = "/usr/bin/chromium",
                Args = new[] { "--no-sandbox" } // Add this line to disable sandboxing (may help in some environments)
            };

            using (var browser = await Puppeteer.LaunchAsync(launchOptions))
            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(htmlContent);

                var pdfBytes = await page.PdfDataAsync();

                return pdfBytes;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }
}