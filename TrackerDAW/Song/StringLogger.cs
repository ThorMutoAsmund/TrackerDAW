using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class StringLogger : Microsoft.Build.Logging.ConsoleLogger
    {
        private string savedLog;

        public string Log => this.savedLog;

        public StringLogger() : base()
        {
            base.WriteHandler = this.SaveLog;
        }

        void SaveLog(string message)
        {
            savedLog += message;
        }

    }
}
