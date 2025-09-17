using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Company.DAL.Entites;

namespace Company.DAL.Data
{
    public static class CompanyDbContextDataSeeding
    {
        public static async Task SeedingDataAsync(CompanyDbContext companyDbContext)
        {
            if(!companyDbContext.Departments.Any())
            {
                var textFile = File.ReadAllText("../Company.DAL/Data/JsonData/Departments.json");
                var departments = JsonSerializer.Deserialize<List<Department>>(textFile);
                await companyDbContext.AddRangeAsync(departments);    
            }
            await companyDbContext.SaveChangesAsync();
        }
    }
}
