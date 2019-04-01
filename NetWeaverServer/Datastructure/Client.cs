namespace NetWeaverServer.Datastructure
{
    public class Client
    {
        public string Hostname { get; }

        public string MACAddress { get; }

        public string IPAddress { get; }

        public Client(string hostname, string macAddress, string ipAddress)
        {
            Hostname = hostname;
            MACAddress = macAddress;
            ipAddress = ipAddress;
        }
        
    }
}