using System.Text.Json;

namespace CodeLearningSpectaclesAPI.Auth
{
  public class RequestFilter
  {

    private readonly RequestDelegate _next;

    public RequestFilter(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      // Filter requests with access_token

      string? authHeader = context.Request.Headers["Authorization"];
      if (authHeader != null && authHeader.StartsWith("Bearer"))
      {
        string access_token = authHeader.Split(' ')[1];
        using var client = new HttpClient();
        string url = "https://api.github.com/user";
        HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, url);
        msg.Headers.Add("User-Agent", "CodeLearningSpectaclesAPI");
        msg.Headers.Add("Authorization", "Bearer " + access_token);
        HttpResponseMessage response = await client.SendAsync(msg);

        if (response.IsSuccessStatusCode)
        {
          string content = await response.Content.ReadAsStringAsync();
          var auth = JsonSerializer.Deserialize<AuthObject>(content);
          context.Items["User"] = auth;
          await _next.Invoke(context);
        }
        else
        {
          context.Response.StatusCode = StatusCodes.Status401Unauthorized;
          context.Response.Headers.Append("Content-Type", "application/json");
          await context.Response.WriteAsync(JsonSerializer.Serialize(new
          {
            Message = "Unauthorized"
          }));
          return;
        }
      }
      else
      {
        // No authorization header
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers.Append("Content-Type", "application/json");
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
          Message = "Unauthorized"
        }));
        return;
      }

    }

  }
}
