using System;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace kulturServer.Models
{
    class TwitterAccount
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string consumerKey { get; set; }
        public string consumerSecret { get; set; }
        public string accessToken { get; set; }
        public string accessTokenSecret { get; set; }

        [Required]
        public virtual iRobotCreate iRobotCreate { get; set; }

        public DateTime TimeAdded { get; set; }
    }
}
