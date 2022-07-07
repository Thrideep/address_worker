using AddressWorker.GeoIp.Api.Controllers;
using AddressWorker.GeoIp.Api.Services;
using Api.Abstractions.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace AddressWorker.GeoIp.Api.Tests.Controllers
{
    public class AddressAnalyzeControllerTests
    {
        private readonly Mock<IGeoIpService> _mockGeoIpService;
        private readonly AddressAnalyzeController _addressAnalyzeController;

        public AddressAnalyzeControllerTests()
        {
            _mockGeoIpService = new Mock<IGeoIpService>();
            _addressAnalyzeController = new AddressAnalyzeController(_mockGeoIpService.Object);
        }

        [Fact]
        public async Task GetResultAsync_should_return_success_for_valid_doamin_address()
        {
            _addressAnalyzeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var geoIpAnalysisResult = new GeoIpAnalysisResult { Message = "test", IsSuccess = true };

            _mockGeoIpService.Setup(e => e.GetResultAsync(It.IsAny<string>(), default)).Returns(Task.FromResult(geoIpAnalysisResult));
            var result = await _addressAnalyzeController.GetResultAsync("sampleAddress") as OkObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result);
            var resultValue = result.Value as GeoIpAnalysisResult;
            Assert.True(resultValue.IsSuccess);
        }

        [Fact]
        public async Task GetResultAsync_should_return_Error_for_invalid_doamin_address()
        {
            _addressAnalyzeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var geoIpAnalysisResult = new GeoIpAnalysisResult { Message = "error", IsSuccess = false };

            _mockGeoIpService.Setup(e => e.GetResultAsync(It.IsAny<string>(), default)).Returns(Task.FromResult(geoIpAnalysisResult));
            var result = await _addressAnalyzeController.GetResultAsync("sampleAddress") as OkObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result);
            var resultValue = result.Value as GeoIpAnalysisResult;
            Assert.False(resultValue.IsSuccess);
        }
    }
}
