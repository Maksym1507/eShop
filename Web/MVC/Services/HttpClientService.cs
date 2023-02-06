using MVC.Services.Abstractions;
using Newtonsoft.Json;
using IdentityModel.Client;

namespace MVC.Services
{
	public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpcontextAccessor;

        public HttpClientService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
		{
			_httpClientFactory = httpClientFactory;
			_httpcontextAccessor = httpContextAccessor;
		}

        public async Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content)
        {
            var client = _httpClientFactory.CreateClient();

            var token = await _httpcontextAccessor.HttpContext.GetTokenAsync("access_token");

            if (!string.IsNullOrEmpty(token))
            {
                client.SetBearerToken(token);
            }

            var httpMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = method
            };

            if (content != null)
            {
                httpMessage.Content =
                    new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            }

            var result = await client.SendAsync(httpMessage);

            if (result.IsSuccessStatusCode)
            {
                var resultContent = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TResponse>(resultContent);
                return response!;
            }

            return default(TResponse) !;
        }
    }
}
