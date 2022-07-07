using AddressWorker.Ping.Api.Models;
using Api.Abstractions.Contracts;
using Api.Abstractions.Extensions;
using Api.Abstractions.Services;
using Api.Abstractions.Validation;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.Rdap.Api.Services
{
    public class RdapService : IRdapService
    {
        private readonly RdapApiOptions _rdapApiOptions;
        private readonly IDataProviderService _dataProviderService;

        public RdapService(RdapApiOptions rdapApiOptions, IDataProviderService dataProviderService)
        {
            _rdapApiOptions = rdapApiOptions;
            _dataProviderService = dataProviderService;
        }

        public async Task<RdapAnalysisResult> GetResultAsync(string type, string address, CancellationToken cancellationToken = default)
        {
            var url = type.ToLower() switch
            {
                "ip" => $"{_rdapApiOptions.BaseUrl}/ip",
                "domain" => $"{_rdapApiOptions.BaseUrl}/domain",
                _ => throw new InvalidAddressTypeException(),
            };


            var data = await _dataProviderService.GetResultAsync($"{url}/{address}", cancellationToken);
            var result = new RdapAnalysisResult { Message = data, IsSuccess = !data.IsNullOrWhiteSpace() };
            return result;
        }
    }
}
