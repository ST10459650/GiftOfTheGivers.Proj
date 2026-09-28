using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Web.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Seed Roles
            string[] roleNames = { "Employee", "Donor", "Admin" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Seed Default Employee Account
            string employeeEmail = "employee@giftofthegivers.org";
            var employeeUser = await userManager.FindByEmailAsync(employeeEmail);

            if (employeeUser == null)
            {
                var newEmployee = new IdentityUser
                {
                    UserName = employeeEmail,
                    Email = employeeEmail,
                    EmailConfirmed = true
                };

                // Password must meet ASP.NET Identity defaults: 8+ chars, uppercase, lowercase, number, special char
                var result = await userManager.CreateAsync(newEmployee, "Employee@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newEmployee, "Employee");
                }
            }
        }
    }
}