using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Api.Abstractions.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data, CancellationToken cancelToken = default)
        {
            var dataAsString = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content, cancelToken);
        }

        public static async Task<T> ReadFromJsonAsync<T>(this HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(stringContent, null, response.StatusCode);
            }
            else
            {
                return JsonSerializer.Deserialize<T>(stringContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
    }
}
