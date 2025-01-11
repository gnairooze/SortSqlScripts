using ScriptsManagement.Contracts;
using Serilog;

namespace ManageFiles
{
    public class Write (ILogger logger, string path) : IWriteFiles
    {
        private readonly ILogger _Logger = logger;
        private readonly string _Path = path;

        public void WriteFile(string fileName, string content)
        {
            _Logger.Debug($"1. Writing file {fileName} started");

            File.WriteAllText(Path.Join(_Path, fileName), content);

            _Logger.Information($"2. Writing file {fileName} completed");
        }
    }
}
