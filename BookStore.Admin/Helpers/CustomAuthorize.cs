using Blazored.LocalStorage;
using BookStore.Admin.Models.AuthDtos;
using System.Text.Json;

namespace BookStore.Admin.Helpers;

public class CustomAuthorize
{
    public static async Task<bool> IsAuthorizedAsync(ILocalStorageService localStorage)
    {
        var token = await localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrEmpty(token);
    }

    public static async Task<string> GetTokenAsync(ILocalStorageService localStorage)
    {
        return await localStorage.GetItemAsync<string>("authToken");
    }

    public static async Task<List<string>> GetRoles(ILocalStorageService localStorage)
    {
        var data = await localStorage.GetItemAsync<string>("data");
        if (string.IsNullOrEmpty(data))
        {
            return new List<string>();
        }

        var loginResult = JsonSerializer.Deserialize<LoginResult>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return loginResult.Roles;
    }
}