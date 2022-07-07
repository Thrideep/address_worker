using Api.Abstractions.Contracts;

namespace Api.Abstractions.Workers
{
    /// <summary>
    /// This class is responsible for talking to the ReverseDns API that is running in the background for the aggregator API to use.
    /// </summary>
    public class ReverseDnsWorker : AnalysisWorkerBase<ReverseDnsAnalysisResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Api.Abstractions.Workers.ReverseDnsWorker "/> class.
        /// </summary>
        /// <param name="url">URL of ReverseDns API.</param>
        public ReverseDnsWorker(string url) : base(url)
        {
        }
    }
}
