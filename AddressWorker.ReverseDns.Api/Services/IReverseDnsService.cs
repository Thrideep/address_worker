using Api.Abstractions.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.ReverseDns.Api.Services
{
    public interface IReverseDnsService
    {
        Task<ReverseDnsAnalysisResult> GetResultAsync(string address, CancellationToken cancellationToken = default);
    }
}
