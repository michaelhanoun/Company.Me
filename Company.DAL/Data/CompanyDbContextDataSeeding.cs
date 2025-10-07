using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Company.DAL.Entites;
using Microsoft.AspNetCore.Identity;

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
        public static async Task SeedingUserAsync(CompanyDbContext companyDbContext, UserManager<User>userManager , RoleManager<IdentityRole> roleManager)
        {
            var role = await roleManager.FindByNameAsync("Admin");
            if (role is null)
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            var email = "michelhanoun210@gmail.com";
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
               user = new User() {FName = "Michael",LName = "Hanoun",IsAgree =true ,Email = email,UserName ="MichaelHanoun"};
               await userManager.CreateAsync(user,"P@ssw0rd?");
            }

            if (!await userManager.IsInRoleAsync(user,"Admin"))
               
               await userManager.AddToRoleAsync(user,"Admin");
          

        }
    }
}
