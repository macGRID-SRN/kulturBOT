using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace kulturServer.Models
{
    public class iRobotCreate
    {
        //yes this is stupid but it ~almost~ guarantees a default value\
        private const bool DEFAULT_STATE = true;
        private bool _state = DEFAULT_STATE;

        public int ID { get; set; }

        [DefaultValue(true)]
        public bool Active
        {
            get { return _state; }
            set { _state = value; }
        }

        public string IP_ADDRESS { get; set; }
        public string Name { get; set; }

        public virtual List<Image> Images { get; set; }
        public virtual TwitterAccount TwitterAccount { get; set; }

        public DateTime TimeCreated { get; set; }
    }
}
