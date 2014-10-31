namespace kulturServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExceptionLogging : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExceptionLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Exception = c.String(),
                        StackTrace = c.String(),
                        Source = c.String(),
                        Fault = c.Int(nullable: false),
                        Method = c.String(),
                        Time = c.DateTime(nullable: false),
                        Data = c.String(),
                        Action = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExceptionLogs");
        }
    }
}
