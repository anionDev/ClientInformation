namespace ClientInformationBackend.Core.Model
{
    public record ClientInformationBackendRecord
    {
        public ClientInformationBackendRecord(string ipAddress)
        {
            this.IPAddress = ipAddress;
        }
        /// <remarks>
        /// Only IPv4 supported until now.
        /// </remarks>
        public string IPAddress { get; set; }
        /// <summary>
        /// Represents the country which is <see cref="IPAddress"/> assigned to.
        /// </summary>
        /// <remarks>
        /// This value will be represented according to ISO-3166 alpha 2.
        /// </remarks>
        public string? Country { get; set; }
        /// <summary>
        /// Represents contactinformation regarding to the currently running ClientInformation-instance.
        /// </summary>
        public string? Contact { get; set; }
        /// <summary>
        /// Represents licenseinformation about the usage-conditions regarding to the currently running ClientInformation-instance.
        /// </summary>
        public string? LicenseInformation { get; set; }
    }
}
