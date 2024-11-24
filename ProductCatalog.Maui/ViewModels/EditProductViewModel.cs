using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProductCatalog.Maui.Messages;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace ProductCatalog.Maui.ViewModels;

[QueryProperty(nameof(Product), "Product")]
public partial class EditProductViewModel : BaseProductViewModel
{
    public EditProductViewModel(IProductService productService) : base(productService) { }
    
    [ObservableProperty]
    private Product? _product;

    partial void OnProductChanged(Product? value)
    {
        if (value == null) return;
    
        Name = value.Name;
        Price = value.Price.ToString(CultureInfo.InvariantCulture);
        Quantity = value.Quantity.ToString();
        LoadCategories();
        SelectedCategory = Categories.FirstOrDefault(c => 
            c.Name.Equals(value.Category.Name, StringComparison.OrdinalIgnoreCase));
    }

    [RelayCommand]
    private async Task Save()
    {
        if (Product == null) return;
        
        var validation = await ValidateProduct();
        if (!validation.IsValid) return;

        var result = _productService.UpdateProduct(new Product
        {
            Id = Product.Id,
            Name = Name,
            Price = validation.Price,
            Quantity = validation.Quantity,
            Category = validation.Category!
        });

        if (result.Success)
        {
            WeakReferenceMessenger.Default.Send(new ProductUpdatedMessage(result.Result!));
            await Shell.Current.DisplayAlert("Success", result.Message, "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", 
                result.Message ?? "Failed to update product", "OK");
        }
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}