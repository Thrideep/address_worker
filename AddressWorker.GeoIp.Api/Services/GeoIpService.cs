using AddressWorker.GeoIp.Api.Models;
using Api.Abstractions.Contracts;
using Api.Abstractions.Extensions;
using Api.Abstractions.Services;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.GeoIp.Api.Services
{
    public class GeoIpService : IGeoIpService
    {
        private readonly GeoIpOptions _geoIpOptions;
        private readonly IDataProviderService _dataProviderService;

        public GeoIpService(GeoIpOptions geoIpOptions, IDataProviderService dataProviderService)
        {
            _geoIpOptions = geoIpOptions;
            _dataProviderService = dataProviderService;
        }

        public async Task<GeoIpAnalysisResult> GetResultAsync(string address, CancellationToken cancellationToken = default)
        {
            var data = await _dataProviderService.GetResultAsync($"{_geoIpOptions.BaseUrl}/{address}/json", cancellationToken);
            var result = new GeoIpAnalysisResult { Message = data, IsSuccess = !data.IsNullOrWhiteSpace() };
            return result;
        }
    }
}
