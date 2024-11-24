using Moq;
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;
using Resources.Shared.Services;

namespace ProductCatalog.Tests.Data;

public class ProductRepository_Tests
{
    private readonly Mock<IDataPersistenceService> _mockDataService;
    private readonly ProductRepository _productRepository;
    private readonly List<Product> _testProducts;

    public ProductRepository_Tests()
    {
        _mockDataService = new Mock<IDataPersistenceService>();
        _productRepository = new ProductRepository(_mockDataService.Object);
        _testProducts = new List<Product>
        {
            new()
            {
                Id = "1",
                Name = "Test Product",
                Category = new Category { Id = "Category1", Name = "Electronics" },
                Price = 100,
                Quantity = 10
            }
        };
    }

    [Fact]
    public void Add_LoadCatalogFails_ReturnsFailure()
    {
        // Arrange
        var newProduct = new Product
        {
            Name = "New Product",
            Category = new Category { Name = "New Category" }
        };

        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Failure<List<Product>>("Failed to load"));

        // Act
        var result = _productRepository.Add(newProduct);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Failed to load product catalog.", result.Message);
    }

    [Fact]
    public void Add_SaveCatalogFails_ReturnsFailure()
    {
        // Arrange
        var newProduct = new Product
        {
            Name = "New Product",
            Category = new Category { Name = "New Category" }
        };

        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(new List<Product>()));
        _mockDataService.Setup(x => x.SaveCatalog(It.IsAny<List<Product>>()))
            .Returns(ResultResponseFactory.Failure<bool>("Failed to save"));

        // Act
        var result = _productRepository.Add(newProduct);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("Failed to save product", result.Message);
    }

    [Fact]
    public void Update_LoadCatalogFails_ReturnsFailure()
    {
        // Arrange
        var product = new Product
        {
            Id = "1",
            Name = "Updated Product"
        };

        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Failure<List<Product>>("Failed to load"));

        // Act
        var result = _productRepository.Update(product);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Failed to load product catalog.", result.Message);
    }

    [Fact]
    public void ProductExists_LoadCatalogFails_ReturnsFailure()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Failure<List<Product>>("Failed to load"));

        // Act
        var result = _productRepository.ProductExists("Test Product");

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Failed to load catalog.", result.Message);
    }

    [Fact]
    public void ProductExists_ProductExists_ReturnsTrue()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));

        // Act
        var result = _productRepository.ProductExists("Test Product");

        // Assert
        Assert.True(result.Success);
        Assert.True(result.Result);
    }

    [Fact]
    public void ProductExists_ProductDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));

        // Act
        var result = _productRepository.ProductExists("Non Existing Product");

        // Assert
        Assert.True(result.Success);
        Assert.False(result.Result);
    }
}

