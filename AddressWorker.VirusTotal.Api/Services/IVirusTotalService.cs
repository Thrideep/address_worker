using Api.Abstractions.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.VirusTotal.Api.Services
{
    public interface IVirusTotalService
    {
        Task<VirusTotalAnalysisResult> GetResultAsync(string apiKey, string address, CancellationToken cancellationToken = default);
    }
}
