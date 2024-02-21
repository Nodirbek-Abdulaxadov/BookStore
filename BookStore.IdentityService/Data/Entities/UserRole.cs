using Microsoft.AspNetCore.Identity;

namespace BookStore.IdentityService.Data.Entities;

public class UserRole
    : IdentityRole
{
    public UserRole()
    {
    }
    public UserRole(string roleName)
        : base(roleName)
    {
    }
}

public static class UserRoles
{
    public const string ADMIN = "Admin";
    public const string SUPER_ADMIN = "SuperAdmin";
    public const string USER = "User";
}