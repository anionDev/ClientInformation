namespace ClientInformationBackend.Core.Model
{
    public record ClientInformationBackendRecord
    {
        /// <remarks>
        /// Only IPv4 supported until now.
        /// </remarks>
        public string? IPAddress { get; set; }
        public string? Country { get; set; }
        public string Contact { get; set; }
        public string LicenseInformation { get; set; }
    }
}
