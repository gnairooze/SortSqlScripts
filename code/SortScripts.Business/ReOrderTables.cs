using ScriptsManagement.Contracts;
using Serilog;

namespace SortScripts.Business
{
    public class ReOrderTables
    {
        public static List<string> ReOrder(ILogger logger, List<string> files, string currentFile, List<string> filesToBeReOrdered, out List<Message> messages)
        {
            logger.Debug($"1. Reordering started for {currentFile} for the dependcies {string.Join(", ", filesToBeReOrdered)}");

            messages = [];

            //Check for Empty or Null Files List:
            //If the files list is null or empty, a warning message is added to messages, and an empty list is returned.
            if (files == null || files.Count == 0)
            {
                logger.Debug("2. No files to reorder");

                messages.Add(new()
                {
                    Text = "No files to reorder",
                    Type = Message.MessageTypes.Warning,
                    Success = true,
                    Code = MessageCodes.NoFilesToReorder

                });
                return [];
            }
            //Reordering Logic:
            //For each file in filesToBeReOrdered:
            var currentPosition = files.IndexOf(currentFile);

            logger.Debug($"2. Current position of {currentFile} is {currentPosition}");

            var counter = 0;
            foreach (var file in filesToBeReOrdered)
            {
                logger.Debug($"3.{counter}.1. Reordering started for dependency {file}");

                //If the file is not found in files, a warning message is added, and the loop continues to the next file.
                if (!files.Contains(file))
                {
                    logger.Debug($"3.{counter}.2. File {file} not found in the list of files");

                    messages.Add(new()
                    {
                        Text = $"File {file} not found in the list of files",
                        Type = Message.MessageTypes.Warning,
                        Success = true,
                        Code = MessageCodes.FileNotFound
                    });
                
                    continue;
                }

                //If the file position is the same as currentPosition, the loop continues to the next file.
                if (files.IndexOf(file) == currentPosition)
                {
                    logger.Debug($"3.{counter}.3. File {file} is already at position {currentPosition}. It is dependant on itself.");

                    messages.Add(new() {
                        Text = $"File {file} is already at position {currentPosition}. It is dependant on itself.",
                        Type = Message.MessageTypes.Info,
                        Success = true,
                        Code = MessageCodes.FileAlreadyAtPosition
                    });
                    continue;
                }

                //If the file's current index is greater than currentPosition, the file is removed from its current position and inserted at currentPosition. An informational message is added to messages.
                if (files.IndexOf(file) > currentPosition)
                {
                    logger.Debug($"3.{counter}.4. File {file} position is greater than the current position {currentPosition}");

                    files.Remove(file);
                    files.Insert(currentPosition, file);

                    logger.Debug($"3.{counter}.5. File {file} moved to position {currentPosition}");

                    messages.Add(new Message()
                    {
                        Text = $"File {file} moved to position {currentPosition}",
                        Type = Message.MessageTypes.Info,
                        Success = true,
                        Code = MessageCodes.FileMoved
                    });

                    currentPosition = files.IndexOf(currentFile);

                    logger.Debug($"3.{counter}.6. The new current position of {currentFile} is {currentPosition} after moving File {file}");
                }

                counter++;
            }

            logger.Debug($"4. Reordering completed for {currentFile} for the dependcies {string.Join(", ", filesToBeReOrdered)}");

            return files;
        }
    }
}
