using AddressWorker.GeoIp.Api.Models;
using AddressWorker.GeoIp.Api.Services;
using Api.Abstractions.Services;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AddressWorker.GeoIp.Api.Tests.Services
{
    public class GeoIpServiceTests
    {
        private readonly GeoIpService _geoIpService;
        private readonly Mock<IDataProviderService> _dataProviderService;

        public GeoIpServiceTests()
        {
            _dataProviderService = new Mock<IDataProviderService>();
            _geoIpService = new GeoIpService(new GeoIpOptions { BaseUrl = "" }, _dataProviderService.Object);
        }

        [Fact]
        public async Task GetResultAsync_should_return_result_for_valid_address()
        {
            _dataProviderService.Setup(e => e.GetResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult("sample result"));
            var result = await _geoIpService.GetResultAsync("8.8.8.8");
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetResultAsync_should_return_result_for_valid_address1()
        {
            _dataProviderService.Setup(e => e.GetResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<string>(null));
            var result = await _geoIpService.GetResultAsync("8888");
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

    }
}
