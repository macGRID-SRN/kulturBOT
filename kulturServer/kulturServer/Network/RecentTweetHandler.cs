using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace kulturServer.Network
{
    class RecentTweetHandler : Handler
    {
        public RecentTweetHandler(byte[] PacketHeader, TcpClient tcpClient) : base(PacketHeader, tcpClient) { }

        private const int MAX_SEND_TWEETS = 200;

        public override bool PerformAction()
        {
            this.SendConfirmPacket();

            using (var db = new Models.Database())
            {
                var robotTweets = db.Robots.First(l => l.ID == this.ROBOT_ID).TwitterAccount.Tweets;

                this.SendSingeBytePacket((byte)Math.Min(robotTweets.Count, MAX_SEND_TWEETS));

                var robotTweetsText = robotTweets.Skip(Math.Max(0, robotTweets.Count() - MAX_SEND_TWEETS)).Take(MAX_SEND_TWEETS).Select(l => l.TweetText).ToList();

                //
                Helpers.Markov.removeSpecialCharacters(robotTweetsText);

                StringBuilder sb = new StringBuilder();

                foreach (var tweet in robotTweetsText)
                {
                    sb.AppendLine(tweet);
                }

                var tempString = sb.ToString();

                var tempBytes = Encoding.ASCII.GetBytes(tempString);

                this.SendAllBytes(tempBytes);

                this.SendConfirmPacket();

                this.CloseConnection();
            }
            return true;
        }
    }
}
