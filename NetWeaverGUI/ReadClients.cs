using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetWeaverServer.Datastructure;

namespace NetWeaverServer.GraphicalUI
{
    public class ReadClients
    {
        public static List<Client> getAllClients()
        {
            List<Client>clients = new List<Client>();
            using (var reader =
                new StreamReader("C:\\Users\\Max\\RiderProjects\\NetWeaver\\NetWeaverGUI\\Data\\mcondb_client.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    
                    clients.Add(new Client(values[0], Int32.Parse(values[3]), values[2], values[1],values[5]=="1", values[4]));
                }
            }
            return clients.OrderBy(c => c.HostName).ToList();
        }
    }

    public class ReadRomms
    {
        public static List<Room> getAllRooms()
        {
            List<Room>rooms = new List<Room>();
            using (var reader =
                new StreamReader("C:\\Users\\Max\\RiderProjects\\NetWeaver\\NetWeaverGUI\\Data\\mcondb_room.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    rooms.Add(new Room(Int32.Parse(values[0]),values[1],values[2],values[3]));
                }
            }

            return rooms.OrderBy(o => o.Roomname).ToList();
        }
    }
}