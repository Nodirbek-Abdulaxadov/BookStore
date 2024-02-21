using BookStore.IdentityService.Data.Entities;
using BookStore.IdentityService.Extend;
using BookStore.IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace BookStore.IdentityService.Services;

public class UserService(UserManager<User> userManager,
                         EmailService emailService)
    : IUserService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly EmailService _emailService = emailService;

    public Task<LoginResult> ChangePasswordAsync(ChangePasswordDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task ConfirmEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new AuthException("User not found");
        }

        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);
    }

    public Task DeleteAccountAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<LoginResult> LoginUserAsync(LoginUserDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task RegisterUserAsync(RegisterUserDto dto, string role)
    {
        if (dto is null)
        {
            throw new AuthException("Register model is null");
        }

        var user = (User)dto;
        var IResult = await _userManager.CreateAsync(user, dto.Password);
        if (!IResult.Succeeded)
        {
            throw new AuthException(IResult.Errors.First().Description);
        }

        IResult = await _userManager.AddToRoleAsync(user, role);
        if (!IResult.Succeeded)
        {
            throw new AuthException(IResult.Errors.First().Description);
        }

        _ = Task.Run(() => _emailService.SendEmail(user.Email!, user.FullName));
    }
}