using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace kulturServer.Helpers
{
    public static class Twitter
    {
        public static async void PostTweetText(int iRobot, string TweetText)
        {
            using (var db = new Models.Database())
            {
                try
                {
                    string UserID;
                    var twitterContext = GetContext(iRobot, out UserID);
                    Status response = await twitterContext.TweetAsync(TweetText);

                    db.SaveChanges();
                }
                //catch (TwitterQueryException e)
                //{
                //    return e.ToString();
                //}
                //catch (InvalidOperationException e)
                //{
                //    return e.ToString();
                //}
                //catch (System.Data.SqlClient.SqlException e)
                //{
                //    return e.ToString();
                //}
                //catch (System.NotSupportedException e)
                //{
                //    return e.ToString();
                //}
                catch (Exception e)
                {
                }
            }
        }

        public static SingleUserAuthorizer GetAuthorization(Models.TwitterAccount TwitterAccount, out string UserID)
        {
            UserID = TwitterAccount.UserID;
            using (var db = new Models.Database())
            {
                return new SingleUserAuthorizer()
                {
                    CredentialStore = new SingleUserInMemoryCredentialStore
                    {
                        ConsumerKey = TwitterAccount.consumerKey,
                        ConsumerSecret = TwitterAccount.consumerSecret,
                        AccessToken = TwitterAccount.accessToken,
                        AccessTokenSecret = TwitterAccount.accessTokenSecret
                    }
                };
            }
        }

        public static TwitterContext GetContext(int iRobotID, out string UserID)
        {
            using (var db = new Models.Database())
            {
                return new TwitterContext(GetAuthorization(db.TwitterAccounts.First(l => l.ID == iRobotID), out UserID));
            }
        }

        public static TwitterContext GetContext(int iRobotID)
        {
            String temp;
            return GetContext(iRobotID, out temp);
        }
    }
}
