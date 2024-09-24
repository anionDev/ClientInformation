namespace ClientInformation.Core.Model
{
    public record ClientInformationRecord
    {
        public string IPAddress { get; set; }
        public string Country { get; set; }
        public string Contact { get; set; }
        public string LicenseInformation { get; set; }
        public bool IsPingable { get; set; }
    }
}
