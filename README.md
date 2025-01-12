# SortSqlScripts
Sort tables' SQL scripts based on dependencies

## configration 
configuration in `appsettings.json`
```json
{
  "LoggerConfiguration": {
    "LogLevel": "Debug",
    "LogFile": "c:\\temp\\logs\\log.txt"
  },

  "SortScriptsConfiguration": {
    "ScriptsPath": "c:\\temp\\scripts",
    "OutputPath": "c:\\temp\\output",
    "Filter": "*.sql"
  }
}
```

1. LoggingConfiguration
    1. LogLevel: Specifies the minimum level of log messages that should be captured. Possible values are: Debug, Information, Warning, Error.
    2. LogFile: Specifies the path of the log file.
2. SortScriptsConfiguration
    1. ScriptsPath: Specifies the path of the folder containing the SQL scripts.
    2. OutputPath: Specifies the path of the folder where the sorted SQL scripts will be saved. The files in this folder will be deleted before the sorted scripts are saved.
    3. Filter: Specifies the filter to be used to search for SQL scripts in the ScriptsPath folder.

## Usage
1. Review the configuration in `appsettings.json`.
2. Execute `SortScripts.exe` from the command line.
