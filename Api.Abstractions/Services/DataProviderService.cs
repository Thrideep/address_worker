using Api.Abstractions.Extensions;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Abstractions.Services
{
    public interface IDataProviderService
    {
        Task<string> GetResultAsync(string url, CancellationToken cancellationToken = default);
        Task<string> PostDataAsync<T>(string uri, T payload, CancellationToken cancellationToken = default) where T : class, new();
    }

    public class DataProviderService : IDataProviderService
    {
        private readonly HttpClient _httpClient;

        public DataProviderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetResultAsync(string url, CancellationToken cancellationToken = default)
        {
            using var response = await _httpClient.GetAsync(url, cancellationToken);
            //return await response.ReadFromJsonAsync<string>();
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                if (responseContent.Contains("RateLimited", StringComparison.OrdinalIgnoreCase))
                {
                    return Constants.RATE_LIMITED;
                }

                return responseContent;
            }

            if (response.StatusCode == 0)
            {
                return Constants.SERVER_OFFLINE;
            }

            return $"{Constants.ERROR_PREFIX}{responseContent}";
        }

        public async Task<string> PostDataAsync<T>(string uri, T payload, CancellationToken cancellationToken = default) where T : class, new()
        {
            using var response = await _httpClient.PostAsJsonAsync(uri, payload, cancellationToken);
            return await response.Content.ReadAsStringAsync(cancellationToken);
            //return await response.ReadFromJsonAsync<string>();
        }
    }
}
