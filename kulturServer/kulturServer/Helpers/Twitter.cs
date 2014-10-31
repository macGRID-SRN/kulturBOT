﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace kulturServer.Helpers
{
    public static class Twitter
    {
        public static async void PostTweetText(int iRobot, string TweetText = "")
        {
            using (var db = new Models.Database())
            {
                try
                {
                    string UserID;
                    var twitterContext = GetContext(iRobot, out UserID);
                    Status response = await twitterContext.TweetAsync(TweetText);

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
                db.SaveChanges();
            }
        }

        public static async void PostTweetWithImage(int iRobotID, int ImageID, string TweetText = "")
        {
            if (string.IsNullOrWhiteSpace(TweetText))
            {
                try
                {
                    TweetText = Helpers.Markov.GetNextTwitterPictureMarkov();
                }
                catch
                {
                    TweetText = "Wasn't able to generate Markov.";
                    System.Diagnostics.Debug.WriteLine("Generating Markov threw an error.");
                }
            }
            using (var db = new Models.Database())
            {
                string UserID;
                var twitterContext = GetContext(iRobotID, out UserID);

                var image = db.Images.FirstOrDefault(l => l.ID == ImageID);

                byte[] imageBytes;

                System.Diagnostics.Debug.WriteLine(Helpers.FileOperations.TextOverlayString(image.FileDirectory));

                if (Helpers.FileOperations.TextOverlayExists(image.FileDirectory))
                {
                    imageBytes = Helpers.FileOperations.GetFileBytes(Helpers.FileOperations.TextOverlayString(image.FileDirectory));
                }
                else
                {
                    imageBytes = Helpers.FileOperations.GetFileBytes(image.FileDirectory);
                }

                try
                {
                    Status tweet = await twitterContext.TweetWithMediaAsync(TweetText, false, imageBytes);
                }
                catch (TwitterQueryException e)
                {
                    System.Diagnostics.Debug.WriteLine("Something went wrong with tweeting that image!");
                    Handlers.ExceptionLogger.LogException(e, Models.Fault.Unknown);
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
