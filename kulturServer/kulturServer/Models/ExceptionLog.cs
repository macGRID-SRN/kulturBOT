using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kulturServer.Models
{
    class ExceptionLog
    {
        public virtual int ID { get; set; }
        public virtual string Message { get; set; }
        public virtual string Exception { get; set; }
        public virtual string StackTrace { get; set; }
        public virtual string Source { get; set; }
        public virtual string Fault { get; set; }
        public virtual string Method { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual string Data { get; set; }
        public virtual string Action { get; set; }
    }

    public enum Fault
    {
        Client, Server, Database, Internet, Twitter, Unknown
    }
}
