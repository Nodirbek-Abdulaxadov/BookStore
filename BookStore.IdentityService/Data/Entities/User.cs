using Microsoft.AspNetCore.Identity;

namespace BookStore.IdentityService.Data.Entities;

public class User : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}