using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kulturServer.Handlers
{
    class ExceptionLogger
    {
        public static void LogException(Exception e, Models.Fault Fault)
        {
            try
            {
                string eInnerString = "No InnerException";
                if (e.InnerException != null)
                    eInnerString = e.InnerException.ToString();
                using (var db = new Models.Database())
                {
                    var exception = new Models.ExceptionLog();
                    exception.Data = e.Data.ToString();
                    exception.Fault = Fault.ToString();
                    exception.Message = e.Message;
                    exception.Time = DateTime.UtcNow;
                    exception.Exception = eInnerString;
                    exception.StackTrace = e.StackTrace;
                    exception.Method = e.TargetSite.Name;
                    exception.Source = e.Source;

                    db.ExceptionLogs.Add(exception);

                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Exception was logged to the database successfully.");
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Failed to log exception to database. We probably broke that too. :(");
            }
        }
    }
}
