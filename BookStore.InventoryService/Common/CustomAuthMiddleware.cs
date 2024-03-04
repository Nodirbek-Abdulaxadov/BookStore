using System.Net;
using System.Net.Http.Headers;

namespace BookStore.InventoryService.Common;

public class CustomAuthMiddleware
{
	private readonly RequestDelegate _next;

	public CustomAuthMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		if (context.Request.Headers.ContainsKey("Authorization"))
		{
			string token = string.Empty;
			try
			{
                token = context.Request.Headers["Authorization"].ToString().Split(" ")[1];
            }
			catch (Exception)
			{
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var response = await httpClient.GetAsync("https://localhost:5001/gateway/auth/check-auth");
			if (response.StatusCode == HttpStatusCode.OK)
			{
				await _next(context);
			}
			else
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync("Unauthorized");
			}
		}
		else
		{
			context.Response.StatusCode = 401;
			await context.Response.WriteAsync("Unauthorized");
		}
	}
}