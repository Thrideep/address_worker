using Api.Abstractions.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.Ping.Api.Services
{
    public interface IPingAnalysisService
    {
        Task<PingAnalysisResult> GetResultAsync(string address, CancellationToken cancellationToken = default);
    }
}
