using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Abstractions.DataProviders
{
    public interface IDataProvider
    {
        Uri GetUrl(string path);
        Uri GetUrl();
        Task<string> GetResultAsync(Uri uri);
        Task<string> PostDataAsync<T>(Uri uri, T payload, CancellationToken cancellationToken = default) where T : class, new();
    }
}
