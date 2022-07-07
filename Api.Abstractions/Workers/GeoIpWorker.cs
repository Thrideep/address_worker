using Api.Abstractions.Contracts;

namespace Api.Abstractions.Workers
{
    /// <summary>
    /// This class is responsible for talking to the Geo API that is running in the background for the aggregator API to use.
    /// </summary>
    public class GeoIpWorker : AnalysisWorkerBase<GeoIpAnalysisResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Api.Abstractions.Workers.GeoIpWorker "/> class.
        /// </summary>
        /// <param name="url">URL of GeoIP API.</param>
        public GeoIpWorker(string url) : base(url)
        {
        }
    }
}
