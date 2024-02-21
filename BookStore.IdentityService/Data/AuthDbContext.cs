using BookStore.IdentityService.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.IdentityService.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<User>(options)
{
    public new DbSet<UserRole> UserRoles { get; set; }
}