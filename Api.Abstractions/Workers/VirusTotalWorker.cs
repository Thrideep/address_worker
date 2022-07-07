using Api.Abstractions.Contracts;

namespace Api.Abstractions.Workers
{
    /// <summary>
    /// This class is responsible for talking to the Virus Total API that is running in the background for the aggregator API to use.
    /// </summary>
    public class VirusTotalWorker : AnalysisWorkerBase<VirusTotalAnalysisResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Api.Abstractions.Workers.VirusTotalWorker"/> class.
        /// </summary>
        /// <param name="url">URL of VirusTotal API.</param>
        public VirusTotalWorker(string url) : base(url)
        {
        }
    }
}
