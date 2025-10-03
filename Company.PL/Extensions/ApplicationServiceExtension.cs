using Company.BLL;
using Company.BLL.Interfaces;
using Company.DAL.Data;
using Company.DAL.Entites;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Company.PL.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddAutoMapper(M=>M.AddProfile<MappingProfile>());
            services.AddControllersWithViews();
            services.AddDbContext<CompanyDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(10);
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;   
            }).AddEntityFrameworkStores<CompanyDbContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/SignIn";
                options.AccessDeniedPath = "/Home/Error";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
      
            });
            return services;
        }
    }
}
