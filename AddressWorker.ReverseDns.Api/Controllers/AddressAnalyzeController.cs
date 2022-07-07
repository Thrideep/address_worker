using AddressWorker.ReverseDns.Api.Services;
using Api.Abstractions;
using Api.Abstractions.Contracts;
using Api.Abstractions.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AddressWorker.ReverseDns.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/analyze")]
    public class AddressAnalyzeController : Controller
    {
        private readonly IReverseDnsService _reverseDnsService;

        public AddressAnalyzeController(IReverseDnsService reverseDnsService)
        {
            _reverseDnsService = reverseDnsService;
        }

        [ProducesResponseType(typeof(ReverseDnsAnalysisResult), 200)]
        [HttpGet("{address}")]
        public async Task<IActionResult> GetResultAsync(string address)
        {

            var isValidAddress = AddressValidator.IsAddressValid(address);
            if (!isValidAddress)
            {
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
                return BadRequest(new ErrorResultModel("Validation Failed", "Invalid Address"));
            }

            var result = await _reverseDnsService.GetResultAsync(address, Request.HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
