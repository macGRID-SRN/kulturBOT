using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace kulturServer.Models
{
    class Database : DbContext
    {
        public Database()
            : base()
        {

        }

        public DbSet<Image> Images { get; set; }
        public DbSet<iRobotCreate> Robots { get; set; }
        public DbSet<TwitterAccount> TwitterAccounts { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<TweetedText> Tweets { get; set; }
    }
}
