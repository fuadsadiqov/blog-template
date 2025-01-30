namespace GP.Core.Utilities
{
    public class IpAddressUtil
    {
        private readonly string baseIpAdress;
        private string[] localIps =
        {
            "::1",
            "127.0.0.1",
            "localhost",
        };
        public IpAddressUtil(string BaseIpAddress)
        {
            baseIpAdress = BaseIpAddress;
        }
        public bool IsLocalIpAddress(string ipAddress)
        {
            return ipAddress.StartsWith(baseIpAdress) ||
                localIps.Contains(ipAddress);
        }
    }
}
