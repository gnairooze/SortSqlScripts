using ScriptsManagement.Contracts;
using ManageFiles;
using Serilog;
using SortScripts.Business;
using Microsoft.Extensions.Configuration;


Console.WriteLine($"{DateTime.UtcNow} - SortScripts started");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var loggerOptions = new SortScripts.Options.LoggerConfiguration(configuration);

Console.WriteLine($"{DateTime.UtcNow} - logger options initialized");

Serilog.Core.Logger logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: LoggerConstants.LogTemplate)
    .WriteTo.File(loggerOptions.LogFile, rollingInterval: RollingInterval.Day, outputTemplate: LoggerConstants.LogTemplate)
    .MinimumLevel.Is((Serilog.Events.LogEventLevel)Enum.Parse(typeof(Serilog.Events.LogEventLevel), loggerOptions.LogLevel))
    .CreateLogger();

Console.WriteLine($"{DateTime.UtcNow} - serilog logger initialized. LogFilePath: {loggerOptions.LogFile}, LogLevel: {loggerOptions.LogLevel}");

var sortScriptsOptions = new SortScripts.Options.SortScriptsConfiguration(configuration);

var scriptsPath = sortScriptsOptions.SriptsPath;
var filter = sortScriptsOptions.Filter;
var outputPath = sortScriptsOptions.OutputPath;

Console.WriteLine($"{DateTime.UtcNow} - sort scripts options initialized. SriptsPath: {sortScriptsOptions.SriptsPath}, Filter: {sortScriptsOptions.Filter}, OutputPath: {sortScriptsOptions.OutputPath}");

IReadFiles readFiles = new Read(logger, scriptsPath, filter);
IWriteFiles writeFiles = new Write(logger, outputPath);
IDeleteFiles deleteFiles = new Delete(logger, outputPath);

var manager = new Manager(readFiles, writeFiles, deleteFiles, logger);

logger.Information("1. SortScripts business manager started");

try
{
    manager.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Error in SortScripts business manager");
    // Ensure to flush and close the log before application exit
    Log.CloseAndFlush();
    throw;
}


logger.Information("2. SortScripts business manager completed");

Console.WriteLine($"{DateTime.UtcNow} - SortScripts completed");

// Ensure to flush and close the log before application exit
Log.CloseAndFlush();
