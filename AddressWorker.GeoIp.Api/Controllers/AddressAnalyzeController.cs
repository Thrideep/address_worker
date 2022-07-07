using AddressWorker.GeoIp.Api.Services;
using Api.Abstractions;
using Api.Abstractions.Extensions;
using Api.Abstractions.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace AddressWorker.GeoIp.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/analyze")]
    public class AddressAnalyzeController : Controller
    {
        private readonly IGeoIpService _geoIpService;

        public AddressAnalyzeController(IGeoIpService geoIpService)
        {
            _geoIpService = geoIpService;
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> GetResultAsync(string address)
        {
            if (address.IsNullOrWhiteSpace())
            {
                return BadRequest(new ErrorResultModel("Validation Failed", new[] { new Error("Get Address Results", "address cannot be empty") }));
            }

            var isValidAddress = AddressValidator.IsAddressValid(address);
            if (!isValidAddress)
            {
                return BadRequest(new ErrorResultModel("Validation Failed", "Invalid Address"));
            }

            var result = await _geoIpService.GetResultAsync(address, Request.HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
