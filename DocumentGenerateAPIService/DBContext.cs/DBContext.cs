using DocumentGenerateAPIService.DBModels;
using Microsoft.EntityFrameworkCore;

namespace DocumentGenerateAPIService.DBContext;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseSqlite(configuration.GetConnectionString("HangfireConnection"));
    }

    public DbSet<FileModel> Files { get; set; }
}
