using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace ProductCatalog.Maui.ViewModels;

public abstract partial class BaseProductViewModel : ObservableObject
{
    protected readonly IProductService _productService;

    [ObservableProperty]
    private ObservableCollection<Category> _categories = [];

    [ObservableProperty]
    private Category? _selectedCategory;

    [ObservableProperty]
    private string _selectedCategoryName = "Choose category";
    
    [ObservableProperty]
    private string _newCategoryName = string.Empty;
    
    [ObservableProperty]
    private bool _isNewCategoryEnabled = true;
    
    [ObservableProperty]
    private bool _isDropdownEnabled = true;
    
    [ObservableProperty]
    private bool _isExpanded;
    
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _price = string.Empty;

    [ObservableProperty]
    private string _quantity = string.Empty;

    protected BaseProductViewModel(IProductService productService)
    {
        _productService = productService;
        LoadCategories();
    }

    protected void LoadCategories()
    {
        var result = _productService.GetAllCategories();
        if (result.Success && result.Result != null)
        {
            Categories = new ObservableCollection<Category>(result.Result);
            OnPropertyChanged(nameof(Categories));
            
            if (!Categories.Any())
            {
                IsNewCategoryEnabled = true;
                SelectedCategory = null;
            }
        }
    }

    partial void OnNewCategoryNameChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            IsNewCategoryEnabled = true;
            IsDropdownEnabled = false;
            SelectedCategory = null;
            SelectedCategoryName = "Choose category";
        }
        else
        {
            IsDropdownEnabled = true;
        }
    }

    [RelayCommand]
    private void SelectCategory(Category category)
    {
        SelectedCategory = category;
        SelectedCategoryName = category.Name;
        NewCategoryName = string.Empty; 
        IsExpanded = false;
    }
    
    partial void OnSelectedCategoryChanged(Category? value)
    {
        OnPropertyChanged(nameof(SelectedCategoryName));
    }

    protected async Task<(bool IsValid, float Price, int Quantity, Category? Category)> ValidateProduct()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Validation Error", "Name is required", "OK");
            return (false, 0, 0, null);
        }

        if (!float.TryParse((string?)Price, NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedPrice))
        {
            await Shell.Current.DisplayAlert("Validation Error", "Invalid price value", "OK");
            return (false, 0, 0, null);
        }

        if (!int.TryParse((string?)Quantity, out int parsedQuantity))
        {
            await Shell.Current.DisplayAlert("Validation Error", "Invalid quantity value", "OK");
            return (false, 0, 0, null);
        }

        
        // Category
        // Got som help from gpt to implement this validation logik 
        Category category;
        if (SelectedCategory != null)
        {
            // Existing category
            category = new Category 
            { 
                Id = SelectedCategory.Id,
                Name = SelectedCategory.Name 
            };
        }
        else if (!string.IsNullOrWhiteSpace(NewCategoryName))
        {
            if (Enumerable.Any(Categories, c => c.Name.Equals(NewCategoryName, StringComparison.OrdinalIgnoreCase)))
            {
                await Shell.Current.DisplayAlert("Category Error", 
                    "This category already exists. Please select it from the dropdown menu.", "OK");
                return (false, 0, 0, null);
            }
            category = new Category 
            { 
                Name = NewCategoryName,
                Id = null!
            };
            Debug.WriteLine($"New category created with Name: {category.Name}, Id: {category.Id}");
        }
        else
        {
            await Shell.Current.DisplayAlert("Validation Error", 
                "Please select an existing category or create a new one", "OK");
            return (false, 0, 0, null);
        }
        return (true, parsedPrice, parsedQuantity, category);
    }
}
