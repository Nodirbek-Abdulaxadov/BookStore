using BookStore.Core.Helpers;
using BookStore.IdentityService.Data.Entities;
using BookStore.IdentityService.Extend;
using BookStore.IdentityService.Models;
using BookStore.IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.IdentityService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController(IUserService userService)
    : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("users")]
    [Authorize(Roles = $"{UserRoles.SUPER_ADMIN}, {UserRoles.ADMIN}")]
    public async Task<IActionResult> GetUsersAsync()
    {
        var users = await _userService.GetUsersAsync(UserRoles.USER);
        return Ok(users);
    }

    [HttpGet("users/{id}")]
    [Authorize(Roles = $"{UserRoles.SUPER_ADMIN}, {UserRoles.ADMIN}")]
    public async Task<IActionResult> GetUserAsync(string id)
    {
        var users = await _userService.GetUserByIdAsync(id);
        return Ok(users);
    }

    [HttpGet("admins")]
    [Authorize(Roles = $"{UserRoles.SUPER_ADMIN}")]
    public async Task<IActionResult> GetAdminsAsync()
    {
        var users = await _userService.GetUsersAsync(UserRoles.ADMIN);
        return Ok(users);
    }

    [HttpPost("create-admin")]
    [Authorize(Roles = $"{UserRoles.SUPER_ADMIN}")]
    public async Task<IActionResult> CreateAdminAsync(RegisterUserDto dto)
    {
        try
        {
            await _userService.RegisterUserAsync(dto, UserRoles.ADMIN);
            return Ok();
        }
        catch (AuthException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerBot.Log(ex.Message, LogType.Error);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("delete-admin")]
    [Authorize(Roles = $"{UserRoles.SUPER_ADMIN}")]
    public async Task<IActionResult> DeleteAdminAsync(string email)
    {
        try
        {
            await _userService.DeleteAccountAsync(email);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            LoggerBot.Log(ex.Message, LogType.Error);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}