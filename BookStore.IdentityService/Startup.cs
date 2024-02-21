using BookStore.IdentityService.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookStore.IdentityService;

public static class Startup
{
    public static async Task SeedRolesAndUsers(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
         
        string[] roleNames = 
        {
            UserRoles.SUPER_ADMIN,
            UserRoles.ADMIN,
            UserRoles.USER
        };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new UserRole(roleName));
            }
        }

        //add default admin
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        const string adminEmail = "super@admin.uz";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            var superAdmin = new User
            {
                Email = adminEmail,
                UserName = adminEmail,
                EmailConfirmed = true,
                FullName = "Super Admin"
            };

            var createAdmin = await userManager.CreateAsync(superAdmin, "Admin.123$");
            if (createAdmin.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdmin, UserRoles.SUPER_ADMIN);
            }
        }
    }
}
