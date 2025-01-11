using ScriptsManagement.Contracts;
using Serilog;

namespace ManageFiles
{
    public class Read(ILogger logger, string path, string filter) : IReadFiles
    {
        private readonly ILogger _Logger = logger;
        public string DirectoryPath { get; set; } = path;
        public string Filter { get; set; } = filter;

        public List<string> GetFileNames()
        {
            _Logger.Debug($"Reading files from {DirectoryPath} with filter {Filter}");

            return [.. Directory.GetFiles(DirectoryPath, Filter).Select(file => Path.GetFileName(file)).OrderBy(f => f).ToList()];
        }

        public string ReadFile(string fileName)
        {
            _Logger.Debug($"Reading file {fileName}");

            return File.ReadAllText(Path.Join(DirectoryPath, fileName));
        }
    }

}

