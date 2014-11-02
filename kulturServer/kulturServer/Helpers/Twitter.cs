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
        public static async void PostTweetText(int iRobot = 1, string TweetText = "")
        {
            if (string.IsNullOrWhiteSpace(TweetText))
            {
                try
                {
                    TweetText = Helpers.Markov.GetNextTwitterMarkov();
                }
                catch (Exception e)
                {
                    TweetText = "Wasn't able to generate Markov.";
                    System.Diagnostics.Debug.WriteLine("Generating Markov threw an error.");
                    Handlers.ExceptionLogger.LogException(e, Models.Fault.Server);
                }
            }

            using (var db = new Models.Database())
            {
                try
                {
                    string UserID;
                    var twitterContext = GetContext(iRobot, out UserID);
                    Status response = await twitterContext.TweetAsync(TweetText);
                    System.Diagnostics.Debug.WriteLine("Tweet sent successfully: " + TweetText);

                    try
                    {
                        var TwitterAccount = db.Robots.First(l => l.ID == iRobot).TwitterAccount;

                        var tweetText = new Models.TweetedText()
                        {
                            TweetText = TweetText,
                            TweetID = response.StatusID.ToString(),
                            TimeAdded = DateTime.UtcNow,
                            ImageTweet = false,
                            TwitterAccount = TwitterAccount
                        };

                        db.Tweets.Add(tweetText);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Unable to save Tweet to database.");
                        Handlers.ExceptionLogger.LogException(e, Models.Fault.Server);
                    }

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
                    Handlers.ExceptionLogger.LogException(e, Models.Fault.Unknown);
                }
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
                catch (Exception e)
                {
                    TweetText = "Wasn't able to generate Markov.";
                    System.Diagnostics.Debug.WriteLine("Generating Markov threw an error.");
                    Handlers.ExceptionLogger.LogException(e, Models.Fault.Server);
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
                    System.Diagnostics.Debug.WriteLine("Tweet with image sent successfully: " + TweetText);

                    try
                    {
                        var TwitterAccount = db.Robots.First(l => l.ID == iRobotID).TwitterAccount;

                        var tweetText = new Models.TweetedText()
                        {
                            TweetText = TweetText,
                            TweetID = tweet.StatusID.ToString(),
                            TimeAdded = DateTime.UtcNow,
                            ImageTweet = true,
                            TwitterAccount = TwitterAccount
                        };

                        db.Tweets.Add(tweetText);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Unable to save Tweet to database.");
                        Handlers.ExceptionLogger.LogException(e, Models.Fault.Server);
                    }
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
