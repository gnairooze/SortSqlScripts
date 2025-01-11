using ScriptsManagement.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageFiles
{
    public class Delete (ILogger logger, string path) : IDeleteFiles
    {
        private readonly ILogger _Logger = logger;
        public string DirectoryPath { get; set; } = path;

        public void DeleteFiles()
        {
            _Logger.Debug($"1. Deleting files from {DirectoryPath} started");

            Directory.GetFiles(DirectoryPath, "*.*").ToList().ForEach(File.Delete);

            _Logger.Debug($"2. Deleting files from {DirectoryPath} completed");
        }
    }
}
