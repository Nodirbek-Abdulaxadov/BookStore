namespace BookStore.IdentityService.Extend;

public class NotFoundException(string message)
    : Exception(message)
{
}