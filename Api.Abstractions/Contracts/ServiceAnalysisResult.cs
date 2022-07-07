using Newtonsoft.Json;

namespace Api.Abstractions.Contracts
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ServiceAnalysisResult
    {
        /// <summary>
        /// Indicates the GeoIP service result.
        /// </summary>
        public GeoIpAnalysisResult GeoIp { get; set; }

        /// <summary>
        /// Indicates Ping service result.
        /// </summary>
        public PingAnalysisResult Ping { get; set; }

        /// <summary>
        /// Indicates Rdap service result.
        /// </summary>
        public RdapAnalysisResult Rdap { get; set; }

        /// <summary>
        /// Indicates ReverseDns service result.
        /// </summary>
        public ReverseDnsAnalysisResult ReverseDns { get; set; }

        /// <summary>
        /// Indicares VirusTotal service result.
        /// </summary>
        public VirusTotalAnalysisResult VirusTotal { get; set; }

        public TouchAnalysisResult TouchAnalysis { get; set; }
    }
}
