using AddressWorker.ReverseDns.Api.Models;
using Api.Abstractions.Contracts;
using Api.Abstractions.Extensions;
using Api.Abstractions.Services;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.ReverseDns.Api.Services
{
    public class ReverseDnsService : IReverseDnsService
    {
        private readonly ReverseDnsOptions _reverseDnsOptions;
        private readonly IDataProviderService _dataProviderService;

        public ReverseDnsService(ReverseDnsOptions reverseDnsOptions, IDataProviderService dataProviderService)
        {
            _reverseDnsOptions = reverseDnsOptions;
            _dataProviderService = dataProviderService;
        }

        public async Task<ReverseDnsAnalysisResult> GetResultAsync(string address, CancellationToken cancellationToken = default)
        {
            //todo: error handling - return failure messages back to the client.
            var data = await _dataProviderService.GetResultAsync($"{_reverseDnsOptions.BaseUrl}?q={address}", cancellationToken);
            var result = new ReverseDnsAnalysisResult { Message = data, IsSuccess = !data.IsNullOrWhiteSpace() };
            return result;
        }
    }
}
