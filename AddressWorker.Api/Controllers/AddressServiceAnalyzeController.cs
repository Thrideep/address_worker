using AddressWorker.Api.Services;
using Api.Abstractions;
using Api.Abstractions.Contracts;
using Api.Abstractions.Extensions;
using Api.Abstractions.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AddressWorker.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/analyze")]
    public class AddressServiceAnalyzeController : ControllerBase
    {
        private readonly IAddressAnalyzeService _addressAnalyzeService;

        public AddressServiceAnalyzeController(IAddressAnalyzeService addressAnalyzeService)
        {
            _addressAnalyzeService = addressAnalyzeService;
        }

        /// <summary>
        /// Gets the data for the given address by querying Ping and RDAP endpoints by default.
        /// </summary>
        /// <param name="address">Address to query</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ServiceAnalysisResult), 200)]
        [ProducesResponseType(typeof(ErrorResultModel), 400)]
        [ProducesResponseType(typeof(ErrorResultModel), 404)]
        [HttpGet("{address}")]
        [SwaggerOperation(Tags = new[] { "Address Service Analyzer" }, Summary = "Takes IP/domain as input and gathers information about default services - Ping and Rdap.")]
        public async Task<IActionResult> GetResultsForDefaultServicesAsync(string address)
        {
            if (!AddressValidator.IsAddressValid(address))
            {
                return BadRequest(new ErrorResultModel("Validation Failed", "Invalid Address"));
            }

            var result = await GetResultAsync("ping,rdap,geoip,rdns", address);
            if (result is null)
            {
                return NotFound(new ErrorResultModel("Empty response", "No data found for the provided services and address"));
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets the data for the given address by querying the services provided.
        /// </summary>
        /// <param name="servicelist">The comma separated list of services to query.</param>
        /// <param name="address">Address to query.</param>
        /// <returns>The aggregated data of different services analysis result.</returns>
        [ProducesResponseType(typeof(ServiceAnalysisResult), 200)]
        [ProducesResponseType(typeof(ErrorResultModel), 400)]
        [ProducesResponseType(typeof(ErrorResultModel), 404)]
        [HttpGet("{servicelist}/{address}")]
        [SwaggerOperation(Tags = new[] { "Address Service Analyzer" }, Summary = "Takes IP/domain as input and gathers information about the provided address from the supplied list of services.")]
        public async Task<IActionResult> GetResultAsync(string servicelist, string address)
        {
            if (!AddressValidator.IsAddressValid(address))
            {
                return BadRequest(new ErrorResultModel("Validation Failed", "Invalid Address"));
            }

            if (servicelist.IsNullOrWhiteSpace())
            {
                return BadRequest(new ErrorResultModel("Validation Failed", "Service list is empty"));
            }

            var services = servicelist.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(str => str.Trim()).Distinct();
            string virusTotalApiKey = null;

            // get api key, incase of VirusTotal
            if (services.Contains("vt"))
            {
                Request.Headers.TryGetValue("X-VT-Key", out StringValues virusTotalApiKeyValues);
                virusTotalApiKey = virusTotalApiKeyValues.Count == 1 ? virusTotalApiKeyValues.FirstOrDefault() : null;
            }

            var result = await _addressAnalyzeService.RunServiceTasksAndBuildResultsAsync(address, services, virusTotalApiKey, Request.HttpContext.RequestAborted);

            if (result is null)
            {
                return NotFound(new ErrorResultModel("Empty response", "No data found for the provided services and address"));
            }

            return Ok(result);
        }
    }
}
