namespace ClientInformationBackend.Core.Model
{
    public record ClientInformationBackendRecord
    {
        public ClientInformationBackendRecord()
        {
        }
        /// <remarks>
        /// Only IPv4 supported until now.
        /// </remarks>
        public required string IPAddress { get; set; }
        /// <summary>
        /// Represents the country which is <see cref="IPAddress"/> assigned to.
        /// </summary>
        /// <remarks>
        /// This value will be represented according to ISO-3166 alpha 2.
        /// </remarks>
        public string? Country { get; set; }
    }
}
