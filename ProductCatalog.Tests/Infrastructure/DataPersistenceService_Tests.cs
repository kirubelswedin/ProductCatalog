using Moq;
using Newtonsoft.Json;
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;
using Resources.Shared.Services;

namespace ProductCatalog.Tests.Infrastructure;

public class DataPersistenceService_Tests
{
    private readonly Mock<IFileService> _mockFileService;
    private readonly DataPersistenceService _dataPersistenceService;
    private readonly List<Product> _testProducts;

    public DataPersistenceService_Tests()
    {
        _mockFileService = new Mock<IFileService>();
        _dataPersistenceService = new DataPersistenceService(_mockFileService.Object);
        
        _testProducts = new List<Product>
        {
            new()
            {
                Id = "1",
                Name = "Test Product",
                Category = new Category { Id = "category1", Name = "Electronics" },
                Price = 100,
                Quantity = 10
            }
        };
    }

    [Fact]
    public void LoadCatalog_EmptyFile_ReturnsEmptyList()
    {
        // Arrange
        _mockFileService.Setup(x => x.GetFromFile())
            .Returns(ResultResponseFactory.Success<string>(""));

        // Act
        var result = _dataPersistenceService.LoadCatalog();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Result);  
        Assert.Empty(result.Result!);   
        Assert.Equal("New product catalog created.", result.Message);
    }

    [Fact]
    public void LoadCatalog_ValidJson_ReturnsProducts()
    {
        // Arrange
        var json = JsonConvert.SerializeObject(_testProducts);
        _mockFileService.Setup(x => x.GetFromFile())
            .Returns(ResultResponseFactory.Success(json));

        // Act
        var result = _dataPersistenceService.LoadCatalog();

        // Assert
        Assert.True(result.Success);
        Assert.Single(result.Result!);
        Assert.Equal("Test Product", result.Result!.First().Name);
    }

    [Fact]
    public void LoadCatalog_InvalidJson_ReturnsException()
    {
        // Arrange
        _mockFileService.Setup(x => x.GetFromFile())
            .Returns(ResultResponseFactory.Success("invalid json"));

        // Act
        var result = _dataPersistenceService.LoadCatalog();

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Unexpected character", result.Message);
    }

    // Got some help from gpt with this test, tests that products saves correctly
    [Fact]
    public void SaveCatalog_ValidProducts_SavesSuccessfully()
    {
        // Arrange
        _mockFileService.Setup(x => x.SaveToFile(It.IsAny<string>()))
            .Returns(ResultResponseFactory.Success(true));

        // Act
        var result = _dataPersistenceService.SaveCatalog(_testProducts);

        // Assert
        Assert.True(result.Success);
        _mockFileService.Verify(x => x.SaveToFile(
            It.Is<string>(s => s.Contains("Test Product"))), 
            Times.Once);
    }

    [Fact]
    public void SaveCatalog_FileServiceFails_ReturnsFailure()
    {
        // Arrange
        _mockFileService.Setup(x => x.SaveToFile(It.IsAny<string>()))
            .Returns(ResultResponseFactory.Failure<bool>("Save failed"));

        // Act
        var result = _dataPersistenceService.SaveCatalog(_testProducts);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Save failed", result.Message);
    }
}