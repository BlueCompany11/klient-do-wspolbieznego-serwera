using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace client_txt
{
    class Program
    {
        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/8d335c6e-e9f3-4bf1-9997-6951c5528a9a/tcp-listener-is-not-receiving-data-from-tcp-client?forum=csharpgeneral
        public static NetworkStream stream;
        public static TcpClient client;
        static void ForceDisc()
        {
            Thread.Sleep(5000);
            stream.Close();
            client.Close();
            client.Client.Close();
            Environment.Exit(0);
        }
        static void Main(string[] args)
        {
            Thread forceDisc = new Thread(ForceDisc);
            forceDisc.Start();
            string hostname = "172.17.1.168";
            Int32 port = 13000;
            client = new TcpClient(hostname, port);
            DateTime text;
            string socketip = client.Client.LocalEndPoint.ToString();
            while (true)
            {
                Console.WriteLine(socketip);
                text = DateTime.Now;
                Connect(client,hostname, "Pierwsze konto albo drugie, nie wiem " + text.ToString());   //172.17.1.168
                Thread.Sleep(1000);
            }
        }
        static void Connect(TcpClient client,String server, String message)
        {
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                
                stream = client.GetStream();
                stream.Flush();
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                //stream.Close();
                //client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            //Console.WriteLine("\n Press Enter to continue...");
            //Console.Read();
        }
    }
}
