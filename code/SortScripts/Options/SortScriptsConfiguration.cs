using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortScripts.Options
{
    internal class SortScriptsConfiguration(IConfigurationRoot configuration)
    {
        private readonly IConfigurationRoot _Configuration = configuration;

        public string SriptsPath => _Configuration["SortScriptsConfiguration:ScriptsPath"] ?? @"scripts";
        public string Filter => _Configuration["SortScriptsConfiguration:Filter"] ?? "*.sql";
        public string OutputPath => _Configuration["SortScriptsConfiguration:OutputPath"] ?? @"outputPath";
    }
}
