using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace kulturServer.Network
{
    class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 5000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            var TotalByteList = new List<byte>();

            byte[] messageInfo = new byte[3];
            int infoBytesRead = 0;

            try
            {
                infoBytesRead = clientStream.Read(messageInfo, 0, 3);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Client didn't send the 3 init bytes correctly");
            }



            clientStream.Write(new byte[] { 255 }, 0, 1);
            clientStream.Flush();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                TotalByteList.AddRange(message);
                //message has successfully been received
                //ASCIIEncoding encoder = new ASCIIEncoding();
                //System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));

                //byte[] buffer = encoder.GetBytes("Hello Client!");

                //clientStream.Write(buffer, 0, buffer.Length);
                //clientStream.Flush();
            }

            System.Diagnostics.Debug.WriteLine("Received packet of " + TotalByteList.Count + " length.");

            File.WriteAllBytes("test.jpg", TotalByteList.ToArray());

            tcpClient.Close();
        }
    }
}