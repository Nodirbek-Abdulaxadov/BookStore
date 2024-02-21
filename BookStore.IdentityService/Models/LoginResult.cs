using BookStore.IdentityService.Extend;

namespace BookStore.IdentityService.Models;

public class LoginResult : UserDto
{ 
    public string Token { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; } = TimeUz.Now;
}