using Serilog;

namespace SortScripts.Business
{
    public class DetectDependencies
    {
        public static List<string> GetDependencies(ILogger logger, string fileContent)
        {
            logger.Debug("1. Detecting dependencies started");

            var dependentTables = new List<string>();

            var foreignKeyLocation = 0;

            var counter = 0;
            while (foreignKeyLocation != -1)
            {
                foreignKeyLocation = fileContent.IndexOf("FOREIGN KEY", foreignKeyLocation + 1);
                if (foreignKeyLocation != -1)
                {
                    logger.Debug($"3.{counter}.1. Found FOREIGN KEY in column {foreignKeyLocation}");
                    int referencesLocation = fileContent.IndexOf("REFERENCES", foreignKeyLocation);
                    logger.Debug($"3.{counter}.2. Found REFERENCES in column {referencesLocation}");
                    int tableNameEnd = fileContent.IndexOf('(', referencesLocation);
                    logger.Debug($"3.{counter}.3. Found table name end in column {tableNameEnd}");
                    string tableName = fileContent[(referencesLocation + 11)..tableNameEnd].Trim();
                    logger.Debug($"3.{counter}.4. Found table name: {tableName}");
                    tableName = tableName.Replace("[", "").Replace("]", "");
                    logger.Debug($"3.{counter}.5. table name trimmed to be {tableName}");
                    dependentTables.Add(tableName);
                }

                counter++;
            }

            logger.Debug("4. Detecting dependencies completed");

            return dependentTables;
        }
    }
}
