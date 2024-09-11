using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> Register(string name, string email, string password)
    {
        var result = await _httpClient.PostAsJsonAsync("api/auth/register", new { Name = name, Email = email, Password = password });
        return result.IsSuccessStatusCode;
    }

    public async Task<string> Login(string email, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            return result.Token;
        }

        return null;
    }
}

public class LoginResult
{
    public string Token { get; set; }
}
