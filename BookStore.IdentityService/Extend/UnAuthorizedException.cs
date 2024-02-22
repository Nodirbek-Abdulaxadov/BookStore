namespace BookStore.IdentityService.Extend;

public class UnAuthorizedException(string message)
    : Exception(message)
{
}