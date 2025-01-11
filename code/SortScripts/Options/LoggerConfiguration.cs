using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortScripts.Options
{
    internal class LoggerConfiguration(IConfigurationRoot configuration)
    {
        private readonly IConfigurationRoot _Configuration = configuration;

        public string LogLevel => _Configuration["LoggerConfiguration:LogLevel"] ?? ScriptsManagement.Contracts.LoggerConstants.LogLevelError;

        public string LogFile => _Configuration["LoggerConfiguration:LogFile"] ?? @"logs\log.txt";
    }
}
