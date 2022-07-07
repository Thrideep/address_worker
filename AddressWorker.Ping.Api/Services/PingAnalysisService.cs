using AddressWorker.Ping.Api.Models;
using Api.Abstractions.Contracts;
using Api.Abstractions.Extensions;
using Api.Abstractions.Services;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.Ping.Api.Services
{
    public class PingAnalysisService : IPingAnalysisService
    {
        private readonly PingApiOptions _pingApiOptions;
        private readonly IDataProviderService _dataProviderService;

        public PingAnalysisService(PingApiOptions pingApiOptions, IDataProviderService dataProviderService)
        {
            _pingApiOptions = pingApiOptions;
            _dataProviderService = dataProviderService;
        }

        public async Task<PingAnalysisResult> GetResultAsync(string address, CancellationToken cancellationToken = default)
        {
            var data = await _dataProviderService.GetResultAsync($"{_pingApiOptions.BaseUrl}/?host={address}", cancellationToken);
            var result = new PingAnalysisResult { Message = data, IsSuccess = !data.IsNullOrWhiteSpace() };
            return result;
        }
    }
}
