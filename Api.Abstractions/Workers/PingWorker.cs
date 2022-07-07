using Api.Abstractions.Contracts;

namespace Api.Abstractions.Workers
{
    /// <summary>
    /// This class is responsible for talking to the Ping API that is running in the background for the aggregator API to use.
    /// </summary>
    public class PingWorker : AnalysisWorkerBase<PingAnalysisResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Api.Abstractions.Workers.PingWorker "/> class.
        /// </summary>
        /// <param name="url">URL of Ping API.</param>
        public PingWorker(string url) : base(url)
        {
        }
    }
}
