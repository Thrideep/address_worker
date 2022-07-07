using AddressWorker.Rdap.Api.Services;
using Api.Abstractions;
using Api.Abstractions.Extensions;
using Api.Abstractions.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AddressWorker.Rdap.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/analyze")]
    public class AddressAnalyzeController : Controller
    {
        private readonly IRdapService _rdapService;

        public AddressAnalyzeController(IRdapService rdapService)
        {
            _rdapService = rdapService;
        }

        [HttpGet("{type}/{address}")]
        public async Task<IActionResult> GetResultAsync(string type, string address)
        {
            if (type.IsNullOrWhiteSpace() || address.IsNullOrWhiteSpace())
            {
                return BadRequest(new ErrorResultModel("Validation Failed", new[] { new Error("Get Address Results", "address and type cannot be empty") }));
            }

            var isValidAddress = AddressValidator.IsAddressValid(address);
            if (!isValidAddress)
            {
                return BadRequest(new ErrorResultModel("Validation Failed", "Invalid Address"));
            }

            var result = await _rdapService.GetResultAsync(type, address, Request.HttpContext.RequestAborted);
            return Ok(result);
        }
    }
}
