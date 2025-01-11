using SortScripts.Business;
using Serilog;

namespace TestBusiness
{
    public class DetectDependenciesTests
    {
        private readonly ILogger _Logger;

        public DetectDependenciesTests()
        {
            Serilog.Core.Logger logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            _Logger = logger;
        }

        [Fact]
        public void GetDependencies_ShouldReturnCorrectTableNames_WhenForeignKeyExists()
        {
            // Arrange
            string fileContent = @"
                CREATE TABLE Orders (
                    OrderID int NOT NULL,
                    OrderNumber int NOT NULL,
                    FOREIGN KEY (OrderID) 
                    REFERENCES Customers(CustomerID)
                );
                CREATE TABLE Customers (
                    CustomerID int NOT NULL,
                    CustomerName varchar(255) NOT NULL
                );";

            // Act
            List<string> result = DetectDependencies.GetDependencies(_Logger, fileContent);

            // Assert
            Assert.Contains("Customers", result);
        }

        [Fact]
        public void GetDependencies_ShouldReturnEmptyList_WhenNoForeignKeyExists()
        {
            // Arrange
            string fileContent = @"
                CREATE TABLE Orders (
                    OrderID int NOT NULL,
                    OrderNumber int NOT NULL
                );
                CREATE TABLE Customers (
                    CustomerID int NOT NULL,
                    CustomerName varchar(255) NOT NULL
                );";

            // Act
            List<string> result = DetectDependencies.GetDependencies(_Logger, fileContent);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetDependencies_ShouldHandleMultipleForeignKeys()
        {
            // Arrange
            string fileContent = @"
                CREATE TABLE Orders (
                    OrderID int NOT NULL,
                    OrderNumber int NOT NULL,
                    FOREIGN KEY (OrderID) 
                    REFERENCES Customers(CustomerID),
                    FOREIGN KEY (OrderNumber) 
                    REFERENCES Products(ProductID)
                );
                CREATE TABLE Customers (
                    CustomerID int NOT NULL,
                    CustomerName varchar(255) NOT NULL
                );
                CREATE TABLE Products (
                    ProductID int NOT NULL,
                    ProductName varchar(255) NOT NULL
                );";

            // Act
            List<string> result = DetectDependencies.GetDependencies(_Logger, fileContent);

            // Assert
            Assert.Contains("Customers", result);
            Assert.Contains("Products", result);
        }
    }
}