namespace BookStore.IdentityService.Extend;

public static class TimeUz
{
    public static DateTime Now => DateTime.UtcNow.AddHours(5);
}
