using AddressWorker.Ping.Api.Services;
using Api.Abstractions;
using Api.Abstractions.Contracts;
using Api.Abstractions.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AddressWorker.Ping.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/analyze")]
    public class AddressAnalyzeController : Controller
    {
        private readonly IPingAnalysisService _pingAnalysisService;

        public AddressAnalyzeController(IPingAnalysisService pingAnalysisService)
        {
            _pingAnalysisService = pingAnalysisService;
        }

        [ProducesResponseType(typeof(PingAnalysisResult), 200)]
        [ProducesResponseType(typeof(ErrorResultModel), 400)]
        [HttpGet("{address}")]
        public async Task<IActionResult> GetResultAsync(string address)
        {
            var isValidAddress = AddressValidator.IsAddressValid(address);
            if (!isValidAddress)
            {
                return BadRequest(new ErrorResultModel("Validation Failed", "Invalid Address"));
            }

            var result = await _pingAnalysisService.GetResultAsync(address);
            return Ok(result);
        }
    }
}
