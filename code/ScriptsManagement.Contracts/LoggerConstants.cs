using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptsManagement.Contracts
{
    public class LoggerConstants
    {
        public const string LogLevelDebug = "Debug";
        public const string LogLevelInformation = "Information";
        public const string LogLevelWarning = "Warning";
        public const string LogLevelError = "Error";

        public const string LogTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} UTC] [{Level:u3}] {Message:lj}{NewLine}{Exception}";
    }
}
