using BookStore.IdentityService.Data.Entities;
using BookStore.IdentityService.Extend;
using BookStore.IdentityService.Models;
using BookStore.IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.IdentityService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserService userService)
    : ControllerBase
{
    private readonly IUserService userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        try
        {
            await userService.RegisterUserAsync(dto, UserRoles.USER);
            return Ok();
        }
        catch (AuthException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("confirm-email/{email}")]
    public async Task<IActionResult> ConfirmEmail(string email)
    {
        try
        {
            await userService.ConfirmEmailAsync(email);
            var contentResult = new ContentResult
            {
                ContentType = "text/html",
                Content = """
                <!DOCTYPE html>
                <html lang="en">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                    <title>Book Store - Email verification</title>
                </head>
                <body margin-top: 30px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;">
                    <div style="display: flex; justify-content: center; width: 100%;">
                        <div style="width: 50%; display: flex; justify-content: center; background-color: white;">
                            <div>
                                <h1>Thank you!</h1>
                                <h2>Your email address has been confirmed</h2>
                
                                <div style="margin-top: 30px; margin-bottom: 20px; display: flex; justify-content: center;">
                                    <span style="color: rgb(134, 134, 134) " >Book Store - 2024</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </body>
                </html>
                """
            };

            return contentResult;
        }
        catch (AuthException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}