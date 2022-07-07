using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressWorker.Api.Controllers
{
    [Route("Errors")]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;
        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //ControllerBase.StatusCode(500, exceptionDetails.Error?.Message);
            _logger.LogError($"The path {exceptionDetails.Path} threw an exception {exceptionDetails.Error}");

            //return Task.FromResult<IActionResult>(ControllerBase.StatusCode(500, exceptionDetails.Error?.Message));
            return Ok(exceptionDetails.Error?.Message);
        }
    }
}
