using BookStore.IdentityService.Models;

namespace BookStore.IdentityService.Services;

public interface IUserService
{
    Task<List<UserDto>> GetUsersAsync(string role);
    Task<UserDto> GetUserByIdAsync(string id);
    Task RegisterUserAsync(RegisterUserDto dto, string role);
    Task<LoginResult> LoginUserAsync(LoginUserDto dto);
    Task<LoginResult> ChangePasswordAsync(ChangePasswordDto dto);
    Task<LoginResult> UpdateProfileAsync(UpdateProfileDto dto);
    Task ChangeEmailAsync(string email, string newEmail);
    Task LogoutAsync(string email);
    Task DeleteAccountAsync(string email);
    Task ConfirmEmailAsync(string email);
}