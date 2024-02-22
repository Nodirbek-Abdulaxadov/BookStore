using BookStore.IdentityService.Extend;
using BookStore.IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User = BookStore.IdentityService.Data.Entities.User;

namespace BookStore.IdentityService.Services;

public class UserService(UserManager<User> userManager,
                         EmailService emailService,
                         IConfiguration configuration)
    : IUserService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly EmailService _emailService = emailService;
    private readonly IConfiguration _configuration = configuration;

    public async Task ChangeEmailAsync(string email, string newEmail)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        user.Email = newEmail;
        user.UserName = newEmail;
        user.EmailConfirmed = false;

        await _userManager.UpdateAsync(user);
        await _userManager.RemoveAuthenticationTokenAsync(user, "BookStore", "Token");
        _ = Task.Run(() => _emailService.SendEmail(user.Email!, user.FullName));
    }

    public async Task<LoginResult> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var IResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!IResult.Succeeded)
        {
            throw new AuthException(IResult.Errors.First().Description);
        }

        await _userManager.RemoveAuthenticationTokenAsync(user, "BookStore", "Token");
        var roles = await _userManager.GetRolesAsync(user);
        var token = JwtHelper.GenerateJwtToken(user, roles, _configuration);

        await _userManager.SetAuthenticationTokenAsync(user, "BookStore", "Token", token);
        return new LoginResult
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Address = user.Address,
            EmailConfirmed = user.EmailConfirmed,
            Roles = roles.ToList(),
            Token = token
        };
    }

    public async Task ConfirmEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);
    }

    public async Task DeleteAccountAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        await _userManager.RemoveAuthenticationTokenAsync(user, "BookStore", "Token");
        await _userManager.DeleteAsync(user);
    }

    public async Task<UserDto> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var userDto = (UserDto)user;
        userDto.Roles = roles.ToList();
        return userDto;
    }

    public async Task<List<UserDto>> GetUsersAsync(string role)
    {
        List<UserDto> userDtos = new();
        var users = await _userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, role))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = (UserDto)user;
                userDto.Roles = roles.ToList();
                userDtos.Add(userDto);
            }
        }
        return userDtos;
    }

    public async Task<LoginResult> LoginUserAsync(LoginUserDto dto)
    {
        if (dto == null)
        {
            throw new AuthException("Login model is null");
        }

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        if (!user.EmailConfirmed)
        {
            throw new UnAuthorizedException("Email not confirmed");
        }

        var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!validPassword)
        {
            throw new UnAuthorizedException("Invalid password");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = JwtHelper.GenerateJwtToken(user, roles, _configuration);

        await _userManager.SetAuthenticationTokenAsync(user, "BookStore", "Token", token);
    
        var loginResult = new LoginResult()
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Address = user.Address,
            EmailConfirmed = user.EmailConfirmed,
            Roles = roles.ToList(),
            Token = token
        };

        return loginResult;
    }

    public async Task LogoutAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        await _userManager.RemoveAuthenticationTokenAsync(user, "BookStore", "Token");
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

    public async Task<LoginResult> UpdateProfileAsync(UpdateProfileDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        user.FullName = dto.FullName;
        user.Address = dto.Address;

        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var token = JwtHelper.GenerateJwtToken(user, roles, _configuration);
        await _userManager.SetAuthenticationTokenAsync(user, "BookStore", "Token", token);

        return new LoginResult
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Address = user.Address,
            EmailConfirmed = user.EmailConfirmed,
            Roles = roles.ToList(),
            Token = token
        };
    }
}