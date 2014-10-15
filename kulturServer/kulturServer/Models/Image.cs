using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kulturServer.Models
{
    class Image
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

        public string FileDirectory { get; set; }
        
        [Required]
        public virtual iRobotCreate iRobot { get; set; }
        
        public DateTime TimeCreated { get; set; }
        public DateTime? TimeAdded { get; set; }
        public DateTime? TimeTaken { get; set; }

    }
}
