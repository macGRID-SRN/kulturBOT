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

        private const uint TWEET_START_DELAY_SECONDS = 0;
        private const uint TWEET_PERIOD_DELAY_SECONDS = 360;
        private const int TWEET_PERIOD_MIN_SECONDS = 100;
        private const int TWEET_PERIOD_MAX_SECONDS = 600;

        private const bool TWEET_RANDOM_INTERVAL = true;

        private static Timer t = new Timer(MakeMarkovTextTweet, null, TWEET_START_DELAY_SECONDS * 1000, TWEET_PERIOD_DELAY_SECONDS * 1000);

        private static Random randy = new Random();

        public Server()
        {
            if (t != null)
                System.Diagnostics.Debug.WriteLine("First text tweet will occur in {0} second(s).", TWEET_START_DELAY_SECONDS);
            this.tcpListener = new TcpListener(IPAddress.Any, 5000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        private static void MakeMarkovTextTweet(Object state)
        {
            System.Diagnostics.Debug.WriteLine("Sending text tweet from timer.");
            Helpers.Twitter.PostTweetText();
            //change the next occurance time to something within the bounds.
            if (TWEET_RANDOM_INTERVAL)
            {
                int randInt = randy.Next(TWEET_PERIOD_MIN_SECONDS, TWEET_PERIOD_MAX_SECONDS) * 1000;
                System.Diagnostics.Debug.WriteLine("Next tweet is occurring in {0} seconds", randInt / 1000);
                t.Change(randInt, TWEET_PERIOD_MAX_SECONDS * 1000);
            }
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

            byte[] messageInfo = new byte[4];
            int infoBytesRead = 0;

            try
            {
                infoBytesRead = clientStream.Read(messageInfo, 0, 4);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Client didn't send the 4 init bytes correctly");
                Handlers.ExceptionLogger.LogException(e, Models.Fault.Client);
            }

            //find the right type of Handler to make
            var myObj = CommType.GetCommType(messageInfo[1]).Type;

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
        public static CommType UpdateHandler = new CommType(1, typeof(UpdateHandler));


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