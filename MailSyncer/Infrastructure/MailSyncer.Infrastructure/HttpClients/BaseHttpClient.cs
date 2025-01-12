using System.Text;
using System.Text.Json;
using MailSyncer.Infrastructure.HttpClients.Models;

namespace MailSyncer.Infrastructure.HttpClients
{
    public abstract class HttpClientBase
    {
        private readonly HttpClient _httpClient;

        protected HttpClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ResponseWrapper<TResponse>> GetAsync<TResponse>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<ResponseWrapper<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest content)
        {
            var jsonContent = CreateJsonContent(content);
            var response = await _httpClient.PostAsync(url, jsonContent);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<ResponseWrapper> PostAsync<TRequest>(string url, TRequest content)
        {
            var jsonContent = CreateJsonContent(content);
            var response = await _httpClient.PostAsync(url, jsonContent);
            return await HandleResponse(response);
        }

        public async Task<ResponseWrapper<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest content)
        {
            var jsonContent = CreateJsonContent(content);
            var response = await _httpClient.PutAsync(url, jsonContent);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<ResponseWrapper> PutAsync<TRequest>(string url, TRequest content)
        {
            var jsonContent = CreateJsonContent(content);
            var response = await _httpClient.PutAsync(url, jsonContent);
            return await HandleResponse(response);
        }

        public async Task<ResponseWrapper<TResponse>> PatchAsync<TRequest, TResponse>(string url, TRequest content)
        {
            var jsonContent = CreateJsonContent(content);
            var response = await _httpClient.PatchAsync(url, jsonContent);
            return await HandleResponse<TResponse>(response);
        }

        public async Task<ResponseWrapper> PatchAsync<TRequest>(string url, TRequest content)
        {
            var jsonContent = CreateJsonContent(content);
            var response = await _httpClient.PatchAsync(url, jsonContent);
            return await HandleResponse(response);
        }

        public async Task<ResponseWrapper> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return await HandleResponse(response);
        }

        private static async Task<ResponseWrapper<T>> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var data = JsonSerializer.Deserialize<T>(jsonData, options);

                if (data == null)
                    return ResponseWrapper<T>.Fail("Response deserialization returned null.");

                return ResponseWrapper<T>.Success(data);
            }

            var error = await response.Content.ReadAsStringAsync();
            return ResponseWrapper<T>.Fail(error);
        }

        private static async Task<ResponseWrapper> HandleResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return ResponseWrapper.Success();
            }

            var error = await response.Content.ReadAsStringAsync();
            return ResponseWrapper.Fail(error);
        }

        private static StringContent CreateJsonContent<T>(T content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var json = JsonSerializer.Serialize(content);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}