
using Resources.Shared.Factories;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace Resources.Shared.Services;

public class ProductService : IProductService
{
   private readonly IProductRepository _productRepository;
   private readonly ICategoryRepository _categoryRepository;

   public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
   {
       _productRepository = productRepository;
       _categoryRepository = categoryRepository;
   }
   
   public ResultResponse<IEnumerable<Product>> GetAllProducts()
   {
       return _productRepository.GetAll();
   }

   public ResultResponse<Product> GetProduct(string nameOrId)
   {
       return _productRepository.GetOne(nameOrId);
   }

   public ResultResponse<IEnumerable<Category>> GetAllCategories()
   {
       return _categoryRepository.GetAll();
   }
   
   public ResultResponse<Product> AddProduct(Product product)
   {
       try
       {
           var nameValidation = ValidateProductName(product.Name);
           if (!nameValidation.Success) return nameValidation;

           var categoryResult = HandleProductCategory(product.Category);
           if (!categoryResult.Success) 
               return ResultResponseFactory.InvalidData<Product>(categoryResult.Message);
           
           product.Category = categoryResult.Result!;
           return _productRepository.Add(product);
       }
       catch (Exception ex)
       { return ResultResponseFactory.Exception<Product>(ex); }
   }

   public ResultResponse<Product> UpdateProduct(Product product)
   {
       try
       {
           var existingProductValidation = ValidateExistingProduct(product);
           if (!existingProductValidation.Success) return existingProductValidation;

           var nameValidation = ValidateProductNameForUpdate(product);
           if (!nameValidation.Success) return nameValidation;

           var categoryResult = HandleProductCategory(product.Category);
           if (!categoryResult.Success)
               return ResultResponseFactory.InvalidData<Product>(categoryResult.Message);

           product.Category = categoryResult.Result!;
           return _productRepository.Update(product);
       }
       catch (Exception ex)
       { return ResultResponseFactory.Exception<Product>(ex); }
   }
   
   public ResultResponse<bool> RemoveProduct(string nameOrId)
   {
       return _productRepository.Delete(nameOrId);
   }
   
   // Validation
   private ResultResponse<Product> ValidateProductName(string name)
   {
       var existsResult = _productRepository.ProductExists(name);
       if (!existsResult.Success)
           return ResultResponseFactory.Failure<Product>(existsResult.Message);

       if (existsResult.Result)
           return ResultResponseFactory.Exists<Product>("A product with the same name already exists.");
       return ResultResponseFactory.Success<Product>(null!);
   }

   private ResultResponse<Product> ValidateExistingProduct(Product product)
   {
       var existingProduct = _productRepository.GetOne(product.Id);
       if (!existingProduct.Success || existingProduct.Result == null)
           return ResultResponseFactory.NotFound<Product>("Product not found.");
       return ResultResponseFactory.Success<Product>(null!);
   }

   private ResultResponse<Product> ValidateProductNameForUpdate(Product product)
   {
       var existingProduct = _productRepository.GetOne(product.Id);
       if (!existingProduct.Result!.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase))
       {
           var nameExists = _productRepository.ProductExists(product.Name);
           if (nameExists.Success && nameExists.Result)
               return ResultResponseFactory.Exists<Product>("A product with the new name already exists.");
       }
       return ResultResponseFactory.Success<Product>(null!);
   }
   
   // Category Handling
   private ResultResponse<Category> HandleProductCategory(Category category)
   {
       if (string.IsNullOrEmpty(category.Id))
           return HandleNewCategory(category);
       return HandleExistingCategory(category);
   }

   private ResultResponse<Category> HandleNewCategory(Category category)
   {
       var existingCategory = _categoryRepository.GetOne(category.Name);
       if (existingCategory.Success && existingCategory.Result != null)
           return ResultResponseFactory.Success(existingCategory.Result);

       category.Id = Guid.NewGuid().ToString();
       return ResultResponseFactory.Success(category);
   }

   private ResultResponse<Category> HandleExistingCategory(Category category)
   {
       var categoryResult = _categoryRepository.GetOne(category.Id);
       if (!categoryResult.Success)
           return ResultResponseFactory.InvalidData<Category>("Invalid Category.");

       if (categoryResult.Result != null)
           return ResultResponseFactory.Success(categoryResult.Result);
       return ResultResponseFactory.InvalidData<Category>("Category not found.");
   }
}