using Api.Abstractions.Extensions;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Abstractions.DataProviders
{
    public abstract class DataProviderBase : IDataProvider
    {
        public abstract string ProviderUrl { get; }
        protected readonly HttpClient httpClient = new();

        public Uri GetUrl(string path)
        {
            if (path.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"Path is required: {nameof(path)}");
            }

            return new Uri($"{ProviderUrl}/{path}");
        }

        public Uri GetUrl()
        {
            return new Uri(ProviderUrl);
        }

        public async Task<string> GetResultAsync(Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            using var response = await httpClient.GetAsync(uri);
            //return await response.ReadFromJsonAsync<string>();

            var responseContent = await response.Content.ReadAsStringAsync();

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

        public async Task<string> PostDataAsync<T>(Uri uri, T payload, CancellationToken cancellationToken = default) where T : class, new()
        {
            using var response = await httpClient.PostAsJsonAsync(uri.ToString(), payload, cancellationToken);
            return await response.Content.ReadAsStringAsync(cancellationToken);
            //return await response.ReadFromJsonAsync<string>();
        }
    }
}
