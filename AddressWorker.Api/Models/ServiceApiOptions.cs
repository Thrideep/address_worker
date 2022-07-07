namespace AddressWorker.Api.Models
{
    public class ServiceApiOptions
    {
        public string RdapApiUrl { get; set; }
        public string GeoIpApiUrl { get; set; }
        public string ReverseDnsApiUrl { get; set; }
        public string PingApiUrl { get; set; }
        public string VirusTotalApiUrl { get; set; }
    }
}
