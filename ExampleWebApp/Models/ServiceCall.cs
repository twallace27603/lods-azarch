using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ExampleWebApp.Models
{
    public static class ServiceCall
    {
        public static string Call(Models.serviceSettings settings)
        {
            string result = string.Empty;
            string hostname = settings.Address;

            int port = settings.Port;

            string command = settings.Command.ToString();

            string message = command;
            if (command == "2")
            {

                message += settings.Minutes.ToString();
            }
            Byte[] output = System.Text.Encoding.ASCII.GetBytes(message);
            using (TcpClient client = new TcpClient(hostname, port))
            {
                using (Stream s = client.GetStream())
                {
                    s.Write(output, 0, output.Length);
                    Byte[] response = new byte[256];
                    s.Read(response, 0, response.Length);
                    result = System.Text.Encoding.ASCII.GetString(response).Trim().Replace("\0", "");
                }
            }
            return result;
        }
    }
}
