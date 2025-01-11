using SortScripts.Business;
using ScriptsManagement.Contracts;
using Serilog;

namespace TestBusiness
{
    public class ReOrderTablesTests
    {
        private readonly ILogger _Logger;

        public ReOrderTablesTests()
        {
            Serilog.Core.Logger logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            _Logger = logger;
        }

        [Fact]
        public void ReOrder_ShouldReturnEmptyList_WhenFilesListIsEmpty()
        {
            // Arrange
            List<string> files = [];
            var currentFile = "file1.sql";
            List<string> filesToBeReOrdered = ["file1.sql"];

            // Act
            var result = ReOrderTables.ReOrder(_Logger, files, currentFile, filesToBeReOrdered, out List<Message> messages);

            // Assert
            Assert.Empty(result);
            Assert.Single(messages);
            Assert.Equal("No files to reorder", messages[0].Text);
            Assert.Equal(Message.MessageTypes.Warning, messages[0].Type);
        }

        [Fact]
        public void ReOrder_ShouldAddWarningMessage_WhenFileNotFound()
        {
            // Arrange
            List<string> files = ["file1.sql", "file2.sql"];
            var currentFile = "file2.sql";
            List<string> filesToBeReOrdered = ["file3.sql"];

            // Act
            var result = ReOrderTables.ReOrder(_Logger, files, currentFile, filesToBeReOrdered, out List<Message> messages);

            // Assert
            Assert.Equal(files, result);
            Assert.Single(messages);
            Assert.Equal("File file3.sql not found in the list of files", messages[0].Text);
            Assert.Equal(Message.MessageTypes.Warning, messages[0].Type);
        }

        [Fact]
        public void ReOrder_ShouldMoveFileToCurrentPosition_WhenFileIsFound()
        {
            // Arrange
            List<string> files = ["file1.sql", "file2.sql", "file3.sql"];
            var currentFile = "file2.sql";
            List<string> filesToBeReOrdered = ["file3.sql"];

            // Act
            var result = ReOrderTables.ReOrder(_Logger, files, currentFile, filesToBeReOrdered, out List<Message> messages);

            // Assert
            Assert.Equal(["file1.sql", "file3.sql", "file2.sql"], result);
            Assert.Single(messages);
            Assert.Equal("File file3.sql moved to position 1", messages[0].Text);
            Assert.Equal(Message.MessageTypes.Info, messages[0].Type);
        }

        [Fact]
        public void ReOrder_ShouldHandleMultipleFilesToBeReOrdered()
        {
            // Arrange
            List<string> files = ["file1.sql", "file2.sql", "file3.sql"];
            var currentFile = "file2.sql";
            List<string> filesToBeReOrdered = ["file3.sql", "file1.sql"];

            // Act
            var result = ReOrderTables.ReOrder(_Logger, files, currentFile, filesToBeReOrdered, out List<Message> messages);

            // Assert
            Assert.Equal(["file1.sql", "file3.sql", "file2.sql"], result);
            Assert.Single(messages);
            Assert.Equal("File file3.sql moved to position 1", messages[0].Text);
            Assert.All(messages, message => Assert.Equal(Message.MessageTypes.Info, message.Type));
        }
    }
}
