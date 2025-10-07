using Company.DAL.Data;
using Company.DAL.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Extensions
{
    public static class AppExtension
    {
        public static async Task GetServiceRequiredServicesMigrateAndUserSeeding(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var context = serviceProvider.GetRequiredService<CompanyDbContext>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var userManger = serviceProvider.GetService<UserManager<User>>();
            var roleManger = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try
            {
                await context.Database.MigrateAsync();
                await CompanyDbContextDataSeeding.SeedingUserAsync(context, userManger, roleManger);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex.Message, ex.StackTrace);
            }

        }
    }
}
