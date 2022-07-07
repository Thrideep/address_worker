using Api.Abstractions.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.GeoIp.Api.Services
{
    public interface IGeoIpService
    {
        Task<GeoIpAnalysisResult> GetResultAsync(string address, CancellationToken cancellationToken = default);
    }
}
