using Company.BLL;
using Company.BLL.Interfaces;
using Company.DAL.Data;
using Company.PL.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Company.PL.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddAutoMapper(M=>M.AddProfile<MappingProfile>());
            services.AddControllersWithViews();
            services.AddDbContext<CompanyDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            return services;
        }
    }
}
