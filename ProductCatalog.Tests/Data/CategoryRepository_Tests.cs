using Moq;
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;
using Resources.Shared.Services;

namespace ProductCatalog.Tests.Data;

public class CategoryRepository_Tests
{
    private readonly Mock<IDataPersistenceService> _mockDataService;
    private readonly CategoryRepository _categoryRepository;
    private readonly List<Product> _testProducts;
    public CategoryRepository_Tests()
    {
        _mockDataService = new Mock<IDataPersistenceService>();
        _categoryRepository = new CategoryRepository(_mockDataService.Object);
        
        _testProducts = new List<Product>
        {
            new() {
                Id = "1",
                Name = "Test Product",
                Category = new Category { Id = "Category1", Name = "Electronics" }
            }
        };
    }
    
    [Fact]
    public void Add_CategoryAlreadyExists_ReturnsExists()
    {
        // Arrange
        var existingCategory = new Category { Name = "Electronics" };
        var products = new List<Product>
        {
            new() { Category = existingCategory }
        };
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(products));

        // Act
        var result = _categoryRepository.Add(existingCategory);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("A category with this name already exists.", result.Message);
    }
    
    [Fact]
    public void GetAll_WithCategories_ReturnsDistinctCategories()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Category = new Category { Name = "Electronics1" } },
            new() { Category = new Category { Name = "Electronics1" } },
            new() { Category = new Category { Name = "Electronics2" } }
        };
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(products));

        // Act
        var result = _categoryRepository.GetAll();

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, result.Result!.Count());
    }

    [Fact]
    public void GetOne_ExistingCategory_ReturnsCategory()
    {
        // Arrange
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));

        // Act
        var result = _categoryRepository.GetOne("Electronics");

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Electronics", result.Result!.Name);
    }

    // got help from gpt with this test.
    [Fact]
    public void Update_CategoryInUse_UpdatesAllProducts()
    {
        // Arrange
        var updatedCategory = new Category { Id = "Category1", Name = "Updated Electronics" };
        _mockDataService.Setup(x => x.LoadCatalog())
            .Returns(ResultResponseFactory.Success(_testProducts));
        _mockDataService.Setup(x => x.SaveCatalog(It.IsAny<List<Product>>()))
            .Returns(ResultResponseFactory.Success(true));

        // Act
        var result = _categoryRepository.Update(updatedCategory);

        // Assert
        Assert.True(result.Success);
        _mockDataService.Verify(x => x.SaveCatalog(It.Is<List<Product>>(
            p => p.All(prod => prod.Category.Name == "Updated Electronics")
        )), Times.Once);
    }
}