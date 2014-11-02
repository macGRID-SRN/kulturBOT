namespace kulturServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTweetedText : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TweetedTexts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        TweetText = c.String(),
                        ImageTweet = c.Boolean(nullable: false),
                        TweetID = c.String(),
                        TimeAdded = c.DateTime(nullable: false),
                        TwitterAccount_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TwitterAccounts", t => t.TwitterAccount_ID)
                .Index(t => t.TwitterAccount_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TweetedTexts", "TwitterAccount_ID", "dbo.TwitterAccounts");
            DropIndex("dbo.TweetedTexts", new[] { "TwitterAccount_ID" });
            DropTable("dbo.TweetedTexts");
        }
    }
}
