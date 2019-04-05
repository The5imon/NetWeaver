using System;
using System.Collections.Generic;
using System.Globalization;

namespace NetWeaverServer.Datastructure
{
    public class DBDump
    {

        public static List<Client> getClientList(List<List<String>> dataList)
        {
            return parseClientList(Parse(dataList));
        }
        public static List<String> Parse(List<List<String>> dataList)
        {
            List<String> values = new List<string>();

            foreach (var clientss in dataList)
            {
                values.Add(String.Join("~", clientss));
            }

            return values;
        }

        public static List<Client> parseClientList(List<String> dataString)
        {
            List<Client> clients = new List<Client>();

            foreach (var clientData in dataString)
            {
                clients.Add(createClient(clientData));
            }

            return clients;
        }

        public static Client createClient(String clientData)
        {
            string mac = clientData.Split('~')[0];
            string ipAddress = clientData.Split('~')[1];
            string hostName = clientData.Split('~')[2];
            int roomNumber = Int32.Parse(clientData.Split('~')[3]);
            string lastSeen = clientData.Split('~')[4];
            bool isOnline = parseBoolean(clientData.Split('~')[5]);

            return new Client(mac, roomNumber, hostName, ipAddress, isOnline, lastSeen);
        }

        private static bool parseBoolean(String value)
        {
            if (value.Equals("True"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}