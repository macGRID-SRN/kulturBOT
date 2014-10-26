namespace kulturServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTwitterAccounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TwitterAccounts",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        UserID = c.String(),
                        consumerKey = c.String(),
                        consumerSecret = c.String(),
                        accessToken = c.String(),
                        accessTokenSecret = c.String(),
                        TimeAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.iRobotCreates", t => t.ID)
                .Index(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TwitterAccounts", "ID", "dbo.iRobotCreates");
            DropIndex("dbo.TwitterAccounts", new[] { "ID" });
            DropTable("dbo.TwitterAccounts");
        }
    }
}
