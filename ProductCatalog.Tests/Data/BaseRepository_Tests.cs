using Moq;
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;
using Resources.Shared.Services;

namespace ProductCatalog.Tests.Data;

// Got some help from gpt to test the abstract BaseRepository
public class TestRepository : BaseRepository
{
    public TestRepository(IDataPersistenceService dataPersistenceService) 
        : base(dataPersistenceService)
    {
    }

    // Expose protected methods for testing
    public ResultResponse<List<Product>> TestLoadCatalog() => LoadCatalog();
    public ResultResponse<bool> TestSaveCatalog(List<Product> products) => SaveCatalog(products);
    public ResultResponse<Product> TestFindByNameOrId(string nameOrId) => FindByNameOrId(nameOrId);
    public ResultResponse<bool> TestExists(Func<Product, bool> predicate) => Exists(predicate);
}

public class BaseRepository_Tests
{
    private readonly Mock<IDataPersistenceService> _mockDataService;
    private readonly TestRepository _repository;
    private readonly List<Product> _testProducts;

    public BaseRepository_Tests()
    {
        _mockDataService = new Mock<IDataPersistenceService>();
        _repository = new TestRepository(_mockDataService.Object);
        _testProducts = new List<Product>
        {
            new()
            {
                Id = "1",
                Name = "Test Product",
                Category = new Category { Id = "category1", Name = "Electronics" }
            }
        };
    }

    [Fact]
    public void LoadCatalog_Success_ReturnsProducts()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));

        // Act
        var result = _repository.TestLoadCatalog();

        // Assert
        Assert.True(result.Success);
        Assert.Single(result.Result!);
    }

    [Fact]
    public void LoadCatalog_Failure_ReturnsFailure()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Failure<List<Product>>("Load failed"));

        // Act
        var result = _repository.TestLoadCatalog();

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Load failed", result.Message);
    }

    [Fact]
    public void SaveCatalog_Success_ReturnsTrue()
    {
        // Arrange
        _mockDataService.Setup(x => x.SaveCatalog(It.IsAny<List<Product>>()))
            .Returns(ResultResponseFactory.Success(true));

        // Act
        var result = _repository.TestSaveCatalog(_testProducts);

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Result);
    }

    [Fact]
    public void FindByNameOrId_ById_ReturnsProduct()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));

        // Act
        var result = _repository.TestFindByNameOrId("1");

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Test Product", result.Result!.Name);
    }

    [Fact]
    public void FindByNameOrId_ByName_ReturnsProduct()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));

        // Act
        var result = _repository.TestFindByNameOrId("Test Product");

        // Assert
        Assert.True(result.Success);
        Assert.Equal("1", result.Result!.Id);
    }

    [Fact]
    public void FindByNameOrId_NotFound_ReturnsNotFound()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));

        // Act
        var result = _repository.TestFindByNameOrId("Non Existing");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Item not found.", result.Message);
    }

    [Fact]
    public void Exists_PredicateTrue_ReturnsTrue()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));

        // Act
        var result = _repository.TestExists(p => p.Name == "Test Product");

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Result);
    }

    [Fact]
    public void Exists_PredicateFalse_ReturnsFalse()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));

        // Act
        var result = _repository.TestExists(p => p.Name == "Non Existing");

        // Assert
        Assert.True(result.Success);
        Assert.False(result.Result);
    }
}