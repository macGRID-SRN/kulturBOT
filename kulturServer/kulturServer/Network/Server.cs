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

            //find the right type of Handler to make
            var myObj = CommType.GetCommType(messageInfo[0]).Type;

            var myHandler = (Handler)Activator.CreateInstance(myObj, messageInfo, tcpClient);

            myHandler.PerformAction();
        }
    }

    public sealed class CommType
    {
        public readonly Type Type;
        private readonly byte value;

        private static readonly Dictionary<byte, CommType> instance = new Dictionary<byte, CommType>();

        public static CommType ImageHandler = new CommType(0, typeof(ImageHandler));

        private CommType(byte value, Type t)
        {
            //what is this magic?
            instance[value] = this;
            this.Type = t;
            this.value = value;
        }

        public static CommType GetCommType(byte CommTypeByte)
        {
            if (instance.ContainsKey(CommTypeByte))
                return instance[CommTypeByte];
            throw new Exception("Communication Type not found!");
        }

    }
}