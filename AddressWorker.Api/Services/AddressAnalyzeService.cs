using AddressWorker.Api.Models;
using Api.Abstractions;
using Api.Abstractions.Contracts;
using Api.Abstractions.Extensions;
using Api.Abstractions.Services;
using Api.Abstractions.Validation;
using Api.Abstractions.Workers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AddressWorker.Api.Services
{
    public interface IAddressAnalyzeService
    {
        Task<ServiceAnalysisResult> RunServiceTasksAndBuildResultsAsync(string address, IEnumerable<string> services, string apiKey, CancellationToken cancellationToken = default);
    }

    public class AddressAnalyzeService : IAddressAnalyzeService
    {
        private readonly ServiceApiOptions _apiOptions;
        private readonly ILogger<AddressAnalyzeService> _logger;

        public AddressAnalyzeService(ServiceApiOptions apiOptions, ILogger<AddressAnalyzeService> logger)
        {
            _apiOptions = apiOptions;
            _logger = logger;
        }

        /// <summary>
        /// Runs the queries in parallel and builds the analysis result.
        /// </summary>
        /// <param name="address">Address to lookup.</param>
        /// <param name="services">Services to query.</param>
        /// <param name="apiKey">Api Key for VirusTotal.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ServiceAnalysisResult> RunServiceTasksAndBuildResultsAsync(string address, IEnumerable<string> services, string apiKey, CancellationToken cancellationToken = default)
        {
            Dictionary<string, dynamic> parallelTasks = RunServiceTasksAsync(address, services, apiKey);
            _logger.LogInformation("before running api tasks");

            ServiceAnalysisResult result = new();

            foreach (var task in parallelTasks)
            {
                dynamic value = await task.Value;

                switch (task.Key)
                {
                    case "vt":
                        result.VirusTotal = value;
                        break;
                    case "rdap":
                        result.Rdap = value;
                        break;
                    case "rdns":
                        result.ReverseDns = value;
                        break;
                    case "ping":
                        result.Ping = value;
                        break;
                    case "geoip":
                        result.GeoIp = value;
                        break;
                    case "touch":
                        result.TouchAnalysis = value;
                        break;
                }
            }

            return result;
        }

        private Dictionary<string, dynamic> RunServiceTasksAsync(string address, IEnumerable<string> services, string apiKey)
        {
            Dictionary<string, dynamic> parallelTasks = new();

            foreach (string service in services)
            {
                switch (service)
                {
                    case "vt":
                        parallelTasks.Add(service, GetVirusTotalDataAsync(address, apiKey));
                        break;
                    case "rdap":
                        parallelTasks.Add(service, GetRdapDataAsync(address));
                        break;
                    case "rdns":
                        parallelTasks.Add(service, GetReverseDnsDataAsync(address));
                        break;
                    case "ping":
                        parallelTasks.Add(service, GetPingDataAsync(address));
                        break;
                    case "geoip":
                        parallelTasks.Add(service, GetGeoIpDataAsync(address));
                        break;
                    case "touch":
                        parallelTasks.Add(service, GetGeoIpDataAsync(address));
                        break;
                }
            }

            return parallelTasks;
        }

        /// <summary>
        /// Gets the VirusTotal information from the Virus Total endpoint.
        /// </summary>
        /// <param name="address">Address to query.</param>
        /// <param name="apiKey">VirusTotal api key</param>
        /// <returns></returns>
        private async Task<VirusTotalAnalysisResult> GetVirusTotalDataAsync(string address, string apiKey)
        {
            if (apiKey.IsNullOrWhiteSpace())
            {
                return null;
            }

            VirusTotalWorker virusTotalWorker = new($"{_apiOptions.VirusTotalApiUrl}/api/analyze/{apiKey}/{address}");
            VirusTotalAnalysisResult result = await virusTotalWorker.FetchDataAsync();
            //todo: return exceptions/errors to the client.
            return result;
        }

        /// <summary>
        /// Gets the RDAP information from the Rdap endpoint.
        /// </summary>
        /// <param name="address">Address to query.</param>
        /// <returns>The Rdap results.</returns>
        private async Task<RdapAnalysisResult> GetRdapDataAsync(string address)
        {
            string type = string.Empty;

            switch (AddressValidator.GetAddressType(address))
            {
                case AddressType.Domain:
                    type = "domain";
                    break;
                case AddressType.IPAddress:
                    type = "ip";
                    break;
            }

            RdapWorker rdapWorker = new($"{_apiOptions.RdapApiUrl}/api/analyze/{type}/{address}");
            RdapAnalysisResult result = await rdapWorker.FetchDataAsync();
            //todo: return errors to client.
            return result;
        }

        /// <summary>
        /// Gets the Reverse DNS information from the Reverse DNS endpoint.
        /// </summary>
        /// <param name="address">Address to query.</param>
        /// <returns>The Reverse Dns data.</returns>
        private async Task<ReverseDnsAnalysisResult> GetReverseDnsDataAsync(string address)
        {
            ReverseDnsWorker reverseDnsWorker = new($"{_apiOptions.ReverseDnsApiUrl}/api/analyze/{address}");
            ReverseDnsAnalysisResult result = await reverseDnsWorker.FetchDataAsync();
            //todo: return exceptions/errors to client.
            return result;
        }

        /// <summary>
        /// Gets the Ping information from the Ping endpoint.
        /// </summary>
        /// <param name="address">Address to query.</param>
        /// <returns>The Ping results.</returns>
        private async Task<PingAnalysisResult> GetPingDataAsync(string address)
        {
            PingWorker pingWorker = new($"{_apiOptions.PingApiUrl}/api/analyze/{address}");
            PingAnalysisResult result = await pingWorker.FetchDataAsync();
            //todo: return exceptions/errors to the client.
            return result;
        }

        /// <summary>
        /// Gets the Geo IP information from the Geo IP endpoint.
        /// </summary>
        /// <param name="address">Address to query.</param>
        /// <returns>The geo ip data.</returns>
        private async Task<GeoIpAnalysisResult> GetGeoIpDataAsync(string address)
        {
            GeoIpWorker geoIpWorker = new($"{_apiOptions.GeoIpApiUrl}/api/analyze/{address}");
            GeoIpAnalysisResult result = await geoIpWorker.FetchDataAsync();
            //todo: return exceptions/errors to the client.
            return result;
        }

        //private async Task<TouchAnalysisResult> GetGeoIpDataAsync(string address)
        //{
        //    GeoIpWorker geoIpWorker = new($"{_apiOptions.GeoIpApiUrl}/api/analyze/{address}");
        //    GeoIpAnalysisResult result = await geoIpWorker.FetchDataAsync();
        //    //todo: return exceptions/errors to the client.
        //    return result;
        //}

    }

}
