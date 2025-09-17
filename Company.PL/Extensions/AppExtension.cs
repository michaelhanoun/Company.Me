using Company.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Company.PL.Extensions
{
    public static class AppExtension
    {
        public static async Task GetServiceRequiredServicesMigrateAndDataSeeding(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<CompanyDbContext>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            try
            {
                await context.Database.MigrateAsync();
                await CompanyDbContextDataSeeding.SeedingDataAsync(context);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex.Message, ex.StackTrace);
            }
        }
    }
}
