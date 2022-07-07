using AddressWorker.VirusTotal.Api.Models;
using Api.Abstractions.Contracts;
using Api.Abstractions.Extensions;
using Api.Abstractions.Services;
using Api.Abstractions.Validation;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.VirusTotal.Api.Services
{
    public class VirusTotalService : IVirusTotalService
    {
        private readonly VirusTotalApiOptions _virusTotalApiOptions;
        private readonly IDataProviderService _dataProviderService;

        public VirusTotalService(VirusTotalApiOptions virusTotalApiOptions, IDataProviderService dataProviderService)
        {
            _virusTotalApiOptions = virusTotalApiOptions;
            _dataProviderService = dataProviderService;
        }

        public async Task<VirusTotalAnalysisResult> GetResultAsync(string apiKey, string address, CancellationToken cancellationToken = default)
        {
            Dictionary<string, string> parameters = new()
            {
                { "apikey", apiKey }
            };

            var addressType = AddressValidator.GetAddressType(address);
            string url;
            switch (addressType)
            {
                case AddressType.Domain:
                    {
                        url = $"{_virusTotalApiOptions.BaseUrl}/domain/report";
                        parameters.Add("domain", address);
                    };
                    break;
                case AddressType.IPAddress:
                    {
                        url = $"{_virusTotalApiOptions.BaseUrl}/ip-address/report";
                        parameters.Add("ip", address);
                    }
                    break;
                default:
                    throw new InvalidAddressTypeException();
            }

            var finalUrl = QueryHelpers.AddQueryString(url, parameters);

            string apiResult = null;
            try
            {
                apiResult = await _dataProviderService.GetResultAsync($"{finalUrl}", cancellationToken);
            }
            catch (Exception)
            {
                apiResult = string.Empty;
            }

            var result = new VirusTotalAnalysisResult { IsSuccess = !apiResult.IsNullOrWhiteSpace(), Message = apiResult };
            return result;
        }
    }
}
