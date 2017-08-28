using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace client_txt
{
    class Program
    {
        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/8d335c6e-e9f3-4bf1-9997-6951c5528a9a/tcp-listener-is-not-receiving-data-from-tcp-client?forum=csharpgeneral
        public static NetworkStream stream;
        public static TcpClient client;
        public static string hostname = "172.17.1.168";
        public static Int32 port = 13000;
        public static Byte[] data=new Byte[256];
        //symulator piszacej osoby ktora wylaczy w cywilizowany sposob aplikacje krzyzyk
        static void ForceDisc()
        {
            Thread.Sleep(40000);
            stream.Close();
            client.Close();
            client.Client.Close();
            Environment.Exit(0);
        }
        static void Main(string[] args)
        {
            //Thread forceDisc = new Thread(ForceDisc);
            //forceDisc.Start();
            Action a = () =>
            {
                while (true)
                {
                    int i = 0;
                    if (data != null)
                    {
                        if (stream!= null && stream.DataAvailable == true && data != null)
                        {
                            Thread.Sleep(500);
                            while ((i = stream.Read(data, 0, data.Length)) != 0)  //gdy pojawi sie jakas wiadomosc
                            {
                                
                                Int32 bytes = stream.Read(data, 0, data.Length);
                                string responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                                Console.WriteLine("Wiadomosc Panie: " + responseData);
                            }
                        }
                    }
                }
            };
            Task permaCheck = new Task(a);
            permaCheck.Start();
            try
            {
                client = new TcpClient(hostname, port);
                string socketip = client.Client.LocalEndPoint.ToString();

                //dodatek do obsluzania wyjscia
                handler = new ConsoleEventDelegate(ConsoleEventCallback);
                SetConsoleCtrlHandler(handler, true);

                string nick = client.Client.LocalEndPoint.ToString();
                while (true)
                {
                    //Console.WriteLine(socketip);
                    string msgg = Console.ReadLine();
                    //text = DateTime.Now;
                    //Connect(client, hostname, "ID: "+nick+" " + text.ToString());   //172.17.1.168
                    Connect(client, hostname, "ID: " + nick + " " + msgg);
                    //Thread.Sleep(1000);
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                Console.WriteLine("Serwer wylaczony");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }

        }
        //dodatek do obslugi wyjscia
        //https://stackoverflow.com/questions/4646827/on-exit-for-a-console-application
        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                stream.Close();
                client.Close();
                client.Client.Close();
                Console.WriteLine("Closing...");
                //Thread.Sleep(1000);
            }
            return false;
        }
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        //koniec dodatku do boslugi wyjscia

        static void Connect(TcpClient client, String server, String message)
        {
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                stream = client.GetStream();
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                //stream.Flush();
                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                //data = new Byte[256];

                //// String to store the response ASCII representation.
                //String responseData = String.Empty;

                //// Read the first batch of the TcpServer response bytes.
                //Int32 bytes = stream.Read(data, 0, data.Length);
                //if (bytes != 0)
                //{
                //    Console.WriteLine("Otrzymalem wiadomosc");
                //}
                //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //Console.WriteLine("Received: {0}", responseData);

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
            catch (System.ObjectDisposedException)
            {

            }

            //Console.WriteLine("\n Press Enter to continue...");
            //Console.Read();
        }
    }
}
