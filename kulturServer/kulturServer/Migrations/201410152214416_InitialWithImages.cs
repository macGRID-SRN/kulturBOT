namespace kulturServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialWithImages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        FileDirectory = c.String(),
                        TimeCreated = c.DateTime(nullable: false),
                        TimeAdded = c.DateTime(),
                        TimeTaken = c.DateTime(),
                        iRobot_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.iRobotCreates", t => t.iRobot_ID, cascadeDelete: true)
                .Index(t => t.iRobot_ID);
            
            CreateTable(
                "dbo.iRobotCreates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        IP_ADDRESS = c.String(),
                        Name = c.String(),
                        TimeCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "iRobot_ID", "dbo.iRobotCreates");
            DropIndex("dbo.Images", new[] { "iRobot_ID" });
            DropTable("dbo.iRobotCreates");
            DropTable("dbo.Images");
        }
    }
}
