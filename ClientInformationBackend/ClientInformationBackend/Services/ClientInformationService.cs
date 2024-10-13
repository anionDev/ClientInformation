using ClientInformationBackend.Core.Model;
using GRYLibrary.Core.Exceptions;
using NetTools;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using GUtilities = GRYLibrary.Core.Misc.Utilities;

namespace ClientInformationBackend.Core.Services
{
    public class ClientInformationBackendService : IClientInformationBackendService
    {
        private readonly IDictionary<byte, IList<(IPAddressRange, string)>> _Cache = new Dictionary<byte, IList<(IPAddressRange, string)>>();

        public ClientInformationBackendService()
        {
            this.Initialize();
        }

        public ClientInformationBackendRecord GetClientInformationBackend(string ipAddress)
        {
            if (!IsValidIPAddress(ipAddress))
            {
                throw new BadRequestException($"Invalid IP-Address: \"{ipAddress}\"");
            }
            return new ClientInformationBackendRecord()
            {
                IPAddress = ipAddress,
                Country = this.GetCountry(ipAddress),
                IsPingable = false,//TODO
                Contact = "See /API/Other/Resources/Information/Contact for contact-information",
                LicenseInformation = "See /API/Other/Resources/Information/License for license-information",
            };
        }

        private string? GetCountry(string ipAddressAsString)
        {
            try
            {
                if (IPAddress.TryParse(ipAddressAsString, out IPAddress? ipAddress))
                {
                    return this.GetCountry(ipAddress!);
                }
            }
            catch
            {
                GUtilities.NoOperation();
            }
            return null;
        }

        private string GetCountry(IPAddress ipAddress)
        {
            byte index = byte.Parse(ipAddress.ToString().Split(".")[0]);
            if (index == 10 || index == 127)
            {
                throw new KeyNotFoundException();
            }
            GUtilities.AssertCondition(0 < index);
            while (index > 0)
            {
                var cacheValue = this._Cache[index];
                if (LowerOrEqual(cacheValue.First().Item1.Begin, ipAddress) && LowerOrEqual(ipAddress, cacheValue.Last().Item1.End))
                {
                    //correct cachevalue found
                    return this.SearchInRange(cacheValue, ipAddress);
                }

                index = (byte)(index - 1);
            }
            throw new KeyNotFoundException();
        }

        private string SearchInRange(IList<(IPAddressRange, string)> ranges, IPAddress ipAddress)
        {
            foreach (var range in ranges)
            {
                if (range.Item1.Contains(ipAddress))
                {
                    return range.Item2;
                }
            }
            throw new KeyNotFoundException();
        }

        internal static bool LowerOrEqual(IPAddress begin, IPAddress ipAddress)
        {
            return Compare(begin, ipAddress) <= 0;
        }
        // see https://stackoverflow.com/a/13631626/3905529
        internal static int ToInteger(IPAddress IP)
        {
            byte[] bytes = IP.GetAddressBytes();
            return (bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
        }
        //returns 0 if equal
        //returns 1 if ip1 > ip2
        //returns -1 if ip1 < ip2
        // see https://stackoverflow.com/a/13631626/3905529
        internal static int Compare(IPAddress IP1, IPAddress IP2)
        {
            int ip1 = ToInteger(IP1);
            int ip2 = ToInteger(IP2);
            return (((ip1 - ip2) >> 0x1F) | (int)((uint)(-(ip1 - ip2)) >> 0x1F));
        }
        internal static bool IsValidIPAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }
            return IPAddress.TryParse(ipAddress, out _);
        }

        private void AddToCache(params string[] dataResourceLines)
        {
            /*
            triples are like
163.199.254.0,163.199.254.255,GB
163.199.255.0,163.203.255.255,ZA
163.204.0.0,163.204.255.255,CN
163.205.0.0,163.207.255.255,US
            now this data must be added to a cache in a format where the country can be looked up quickly for a specific ip
             */
            foreach (var plainTriple in dataResourceLines)
            {
                var triple = plainTriple.Split(",");
                var firstOctet1 = byte.Parse(triple[0].Split(".")[0]);
                var range = IPAddressRange.Parse($"{triple[0]} - {triple[1]}");
                this._Cache[firstOctet1].Add((range, triple[2]));
            }
        }

        private void Initialize()
        {
            this.InitializeCache();
            this.LoadData();
        }
        private void LoadData()
        {
            string dataResourceFileContent = LoadDataResource();
            string[] dataResourceLines = dataResourceFileContent.Split("\n").Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            this.AddToCache(dataResourceLines);
        }

        private void InitializeCache()
        {
            for (byte i = 0; i < 255; i++)
            {
                this._Cache.Add(i, new List<(IPAddressRange, string)>());
            }
        }

        private static string LoadDataResource()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream("ClientInformationBackend.Core.Data.GeoIPData.csv");
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
