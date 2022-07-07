using AddressWorker.VirusTotal.Api.Services;
using Api.Abstractions;
using Api.Abstractions.Extensions;
using Api.Abstractions.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AddressWorker.VirusTotal.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/analyze")]
    public class AddressAnalyzeController : Controller
    {
        private readonly IVirusTotalService _virusTotalService;

        public AddressAnalyzeController(IVirusTotalService virusTotalService)
        {
            _virusTotalService = virusTotalService;
        }

        [HttpGet("{apikey}/{address}")]
        public async Task<IActionResult> GetResultAsync(string apikey, string address)
        {
            if (apikey.IsNullOrWhiteSpace())
            {
                return Unauthorized(new ErrorResultModel("Validation Failed", "Api Key is required"));
            }

            var isValidAddress = AddressValidator.IsAddressValid(address);
            if (!isValidAddress)
            {
                return BadRequest(new ErrorResultModel("Validation Failed", "Invalid Address"));
            }

            var result = await _virusTotalService.GetResultAsync(apikey, address, Request.HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
