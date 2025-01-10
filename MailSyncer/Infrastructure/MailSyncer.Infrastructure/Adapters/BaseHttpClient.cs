using System.Text;
using System.Text.Json;

namespace MailSyncer.Infrastructure.Adapters
{
    public abstract class BaseHttpClient
    {
        private readonly HttpClient _httpClient;

        protected BaseHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            return await HandleResponse<TResponse>(response);
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var content = SerializeContent(data);
            var response = await _httpClient.PostAsync(endpoint, content);
            return await HandleResponse<TResponse>(response);
        }

        protected async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var content = SerializeContent(data);
            var response = await _httpClient.PutAsync(endpoint, content);
            return await HandleResponse<TResponse>(response);
        }

        protected async Task<TResponse> PatchAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var content = SerializeContent(data);
            var response = await _httpClient.PatchAsync(endpoint, content);
            return await HandleResponse<TResponse>(response);
        }

        protected async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error deleting resource: {error}");
            }
        }

        private async Task<TResponse> HandleResponse<TResponse>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TResponse>(responseData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result != null ? result : throw new InvalidOperationException("The deserialized response is null.");
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request failed with status {response.StatusCode}: {error}");
        }

        private static StringContent SerializeContent<TRequest>(TRequest data)
        {
            var jsonData = JsonSerializer.Serialize(data);
            return new StringContent(jsonData, Encoding.UTF8, "application/json");
        }
    }
}