using Api.Abstractions.Contracts;

namespace Api.Abstractions.Workers
{
    /// <summary>
    /// This class is responsible for talking to the Rdap API that is running in the background for the aggregator API to use.
    /// </summary>
    public class RdapWorker : AnalysisWorkerBase<RdapAnalysisResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Api.Abstractions.Workers.RdapWorker "/> class.
        /// </summary>
        /// <param name="url">URL of Rdap API.</param>
        public RdapWorker(string url) : base(url)
        {
        }
    }
}
