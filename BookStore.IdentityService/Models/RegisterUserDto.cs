using BookStore.IdentityService.Data.Entities;

namespace BookStore.IdentityService.Models;

public class RegisterUserDto : LoginUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public static implicit operator User(RegisterUserDto dto)
        => new()
        {
            Email = dto.Email,
            UserName = dto.Email,
            FullName = dto.FullName,
            Address = dto.Address
        };
}