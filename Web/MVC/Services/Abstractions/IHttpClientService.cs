namespace MVC.Services.Abstractions
{
	public interface IHttpClientService
	{
		Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content);
	}
}
