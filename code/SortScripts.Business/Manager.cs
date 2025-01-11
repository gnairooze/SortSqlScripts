using ScriptsManagement.Contracts;
using Serilog;

namespace SortScripts.Business
{
    public class Manager (IReadFiles readFiles, IWriteFiles writeFiles, ILogger logger)
    {
        private readonly IReadFiles _ReadFiles = readFiles;
        private readonly IWriteFiles _WriteFiles = writeFiles;
        private readonly ILogger _Logger = logger;
        private int ReOrderFileNamesCount = 0;

        public void Run()
        {
            _Logger.Debug("1. Run started");

            var fileNames = _ReadFiles.GetFileNames();
            _Logger.Debug($"2. Found {fileNames.Count} files to reorder");

            var reOrderedList = ReOrderFileNames(fileNames);

            _Logger.Debug($"3. files reordered");

            var digitsLength = reOrderedList.Count.ToString().Length;

            _Logger.Debug($"4. Digits length: {digitsLength}");

            for (int i = 0; i < reOrderedList.Count; i++)
            {
                var fileName = reOrderedList[i];
                var fileContent = _ReadFiles.ReadFile(fileName);
                var countPrefix = (i + 1).ToString($"D{digitsLength}");
                fileName = $"{countPrefix}-{fileName}";
                _WriteFiles.WriteFile(fileName, fileContent);

                _Logger.Debug($"5.{i + 1}. File {fileName} written");
            }

            _Logger.Debug("6. Run completed");
        }

        private List<string> ReOrderFileNames(List<string> fileNames)
        {
            ReOrderFileNamesCount++;

            _Logger.Debug($"1. ReOrderFileNames started for the {ReOrderFileNamesCount} time");

            var orderedList = new List<string>();

            for (int i = 0; i < fileNames.Count; i++)
            {
                var fileName = fileNames[i];

                _Logger.Debug($"2.{i+1}.1. ReOrderFileNames started for file {fileName}");

                var fileContent = _ReadFiles.ReadFile(fileName);

                _Logger.Debug($"2.{i + 1}.2. ReOrderFileNames read file {fileName}");

                var dependencies = DetectDependencies.GetDependencies(_Logger, fileContent);

                _Logger.Information($"2.{i + 1}.3. ReOrderFileNames detected dependencies for file {fileName}: {string.Join(", ", dependencies)}");

                if (dependencies.Count == 0)
                {
                    _Logger.Debug($"2.{i + 1}.4. ReOrderFileNames no dependencies detected for file {fileName}");
                    continue;
                }

                orderedList = ReOrderTables.ReOrder(_Logger, fileNames, "fileName", dependencies, out List<Message> messages);

                foreach (var message in messages)
                {
                    switch (message.Type)
                    {
                        case Message.MessageTypes.Debug:
                            _Logger.Debug($"2.{i + 1}.4. ReOrderFileNames message: {message.Text}");
                            break;
                        case Message.MessageTypes.Info:
                            _Logger.Information($"2.{i + 1}.4. ReOrderFileNames message: {message.Text}");
                            break;
                        case Message.MessageTypes.Warning:
                            _Logger.Warning($"2.{i + 1}.4. ReOrderFileNames message: {message.Text}");
                            break;
                        case Message.MessageTypes.Error:
                            _Logger.Error($"2.{i + 1}.4. ReOrderFileNames message: {message.Text}");
                            break;
                    }
                }

                _Logger.Debug($"2.{i + 1}.4. ReOrderFileNames reordered files for file {fileName}");

                if (messages.Count > 0 && messages.Any(m => m.Code == MessageCodes.FileMoved))
                {
                    _Logger.Debug($"2.{i + 1}.5. ReOrderFileNames detected that file(s) were moved for file {fileName}. ReOrder will restart.");

                    _Logger.Debug($"2.{i + 1}.6. ReOrderFileNames completed for the {ReOrderFileNamesCount} time");

                    ReOrderFileNames(orderedList);
                }
            }

            _Logger.Debug($"3. ReOrderFileNames completed for the {ReOrderFileNamesCount} time");

            return orderedList;
        }

    }
}
