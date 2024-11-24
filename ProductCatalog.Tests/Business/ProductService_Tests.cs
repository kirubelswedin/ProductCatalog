using Moq;
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;
using Resources.Shared.Services;

namespace ProductCatalog.Tests.Business;

public class ProductService_Tests
{
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly Mock<ICategoryRepository> _mockCategoryRepo;
    private readonly ProductService _productService;
    private readonly List<Product> _testProducts;
    private readonly List<Category> _testCategories;

    public ProductService_Tests()
    {
        _mockProductRepo = new Mock<IProductRepository>();
        _mockCategoryRepo = new Mock<ICategoryRepository>();
        _productService = new ProductService(_mockProductRepo.Object, _mockCategoryRepo.Object);

        _testCategories = new List<Category>
        {
            new() { Id = "category1", Name = "Electronics" }
        };

        _testProducts = new List<Product>
        {
            new()
            {
                Id = "1",
                Name = "Test Product",
                Category = _testCategories[0],
                Price = 100,
                Quantity = 10
            }
        };
    }

    [Fact]
    public void GetAllProducts_Success_ReturnsProducts()
    {
        // Arrange
        _mockProductRepo.Setup(x => x.GetAll())
            .Returns(ResultResponseFactory.Success<IEnumerable<Product>>(_testProducts));

        // Act
        var result = _productService.GetAllProducts();

        // Assert
        Assert.True(result.Success);
        Assert.Single(result.Result!);
    }

    // got some help from gpt with this test, tests add new product with new category
    [Fact]
    public void AddProduct_NewProductNewCategory_Success()
    {
        // Arrange
        var newProduct = new Product
        {
            Name = "New Product",
            Category = new Category { Name = "New Category" },
            Price = 200,
            Quantity = 5
        };

        _mockProductRepo.Setup(x => x.ProductExists("New Product"))
            .Returns(ResultResponseFactory.Success(false));
        
        _mockCategoryRepo.Setup(x => x.GetOne("New Category"))
            .Returns(ResultResponseFactory.NotFound<Category>(""));
        
        _mockProductRepo.Setup(x => x.Add(It.IsAny<Product>()))
            .Returns(ResultResponseFactory.Success(newProduct));

        // Act
        var result = _productService.AddProduct(newProduct);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Result!.Category.Id);
    }

    [Fact]
    public void AddProduct_ExistingProductName_ReturnsExists()
    {
        // Arrange
        var newProduct = new Product { Name = "Test Product" };
        _mockProductRepo.Setup(x => x.ProductExists("Test Product"))
            .Returns(ResultResponseFactory.Success(true));

        // Act
        var result = _productService.AddProduct(newProduct);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("already exists", result.Message);
    }
    
    [Fact]
    public void UpdateProduct_ValidUpdate_Success()
    {
        // Arrange
        var updatedProduct = new Product
        {
            Id = "1",
            Name = "Updated Name",
            Category = _testCategories[0]
        };

        _mockProductRepo.Setup(x => x.GetOne("1"))
            .Returns(ResultResponseFactory.Success(_testProducts[0]));
        
        _mockProductRepo.Setup(x => x.ProductExists("Updated Name"))
            .Returns(ResultResponseFactory.Success(false));
        
        _mockCategoryRepo.Setup(x => x.GetOne("category1"))
            .Returns(ResultResponseFactory.Success(_testCategories[0]));
        
        _mockProductRepo.Setup(x => x.Update(It.IsAny<Product>()))
            .Returns(ResultResponseFactory.Success(updatedProduct));

        // Act
        var result = _productService.UpdateProduct(updatedProduct);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public void UpdateProduct_NonExistentProduct_ReturnsNotFound()
    {
        // Arrange
        var product = new Product { Id = "999" };
        _mockProductRepo.Setup(x => x.GetOne("999"))
            .Returns(ResultResponseFactory.NotFound<Product>(""));

        // Act
        var result = _productService.UpdateProduct(product);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("not found", result.Message);
    }
}