using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kulturServer.Models
{
    public class TweetedText
    {
        private const bool DEFAULT_STATE = true;
        private bool _state = DEFAULT_STATE;

        public int ID { get; set; }

        [DefaultValue(true)]
        public bool Active
        {
            get { return _state; }
            set { _state = value; }
        }

        public string TweetText { get; set; }
        public bool ImageTweet { get; set; }
        public int? TweetID { get; set; }

        public virtual TwitterAccount TwitterAccount { get; set; }
        public DateTime TimeAdded { get; set; }
    }
}
