using BookStore.IdentityService.Data.Entities;

namespace BookStore.IdentityService.Models;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public List<string> Roles { get; set; } = new();


    public static implicit operator UserDto(User user)
        => new()
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Address = user.Address,
            EmailConfirmed = user.EmailConfirmed,
            Roles = new List<string>()
        };
}