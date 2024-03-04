namespace BookStore.Admin.Models.UserDtos;

public class RegisterUser
{
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}