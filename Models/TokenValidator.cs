using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Xml.Linq;
using AuthenticationService;
using AuthenticationService.EntityModel;
using Microsoft.Extensions.Logging;

namespace CustomersAPI
{
    public class TokenValidator
    {
        private readonly ILogger<TokenValidator> _logger;
        public TokenValidator(ILogger<TokenValidator> logger)
        {
            _logger = logger;
        }
        public async Task<AuthResponse> Validate(string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5096");
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync("/api/accounts/validate");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Validation Succeeded");
                    var user = await response.Content.ReadFromJsonAsync<AuthResponse>();
                    return user;
                }
                else
                {
                    _logger.LogError("validation failed");
                }
            }
            return null;
        }
    }
}
