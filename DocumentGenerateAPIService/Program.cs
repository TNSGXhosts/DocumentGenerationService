using AutoMapper;
using DocumentGenerateAPIService.Automapper;
using DocumentGenerateAPIService.ConversionJobs;
using DocumentGenerateAPIService.DBContext;
using DocumentGenerateAPIService.DBModels;
using DocumentGenerateAPIService.FileService;
using Hangfire;
using Hangfire.MemoryStorage;
using PuppeteerSharp;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);
var app = builder.Build();
Configure(app, builder);

await new BrowserFetcher().DownloadAsync();
app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers().AddNewtonsoftJson();
    services.AddCors();
    services.AddHangfire(config => config.UseMemoryStorage());
    services.AddDbContext<AppDbContext>();
    services.AddScoped<IFileService, FileService>();
    services.AddScoped<IFileRepository, FileRepository>();
    services.AddSingleton<IMapper>(new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new AutomapperProfile());
    }).CreateMapper());
    services.AddHangfireServer();
}

void Configure(WebApplication app, WebApplicationBuilder builder)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHangfireDashboard();
    RecurringJob.AddOrUpdate<RecurringConversionJob>(j => j.ConvertHtmlToPdf(null), Cron.Minutely);
    app.UseRouting();
    app.UseCors(bldr => bldr
            .WithOrigins(builder.Configuration["AllowedOrigins"] ?? string.Empty)
            .AllowAnyMethod()
            .AllowAnyHeader());
    app.UseEndpoints(endpoints =>
    {
        _ = endpoints.MapControllers();
    });

    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangfireAuthorizationFilter() }
    });
}
