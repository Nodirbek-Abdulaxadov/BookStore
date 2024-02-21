using BookStore.IdentityService.Models;

namespace BookStore.IdentityService.Services;

public interface IUserService
{
    Task<UserDto> GetUsersAsync();
    Task<UserDto> GetUserByIdAsync(string id);
    Task RegisterUserAsync(RegisterUserDto dto, string role);
    Task<LoginResult> LoginUserAsync(LoginUserDto dto);
    Task<LoginResult> ChangePasswordAsync(ChangePasswordDto dto);
    Task DeleteAccountAsync(string email);
    Task ConfirmEmailAsync(string email);
}