using Api.Abstractions.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.Rdap.Api.Services
{
    public interface IRdapService
    {
        Task<RdapAnalysisResult> GetResultAsync(string addressType, string address, CancellationToken cancellationToken = default);
    }
}
