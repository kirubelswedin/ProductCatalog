using CommunityToolkit.Mvvm.Input;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace ProductCatalog.Maui.ViewModels;

public partial class CreateProductViewModel : BaseProductViewModel
{
    public CreateProductViewModel(IProductService productService) : base(productService) { }

    [RelayCommand]
    private async Task Save()
    {

        var validation = await ValidateProduct();
        if (!validation.IsValid) return;

        var result = _productService.AddProduct(new Product
        {
            Name = Name,
            Price = validation.Price,
            Quantity = validation.Quantity,
            Category = validation.Category!
        });

        if (result.Success)
        {
            await Shell.Current.DisplayAlert("Success", result.Message, "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error",
                result.Message ?? "Failed to add product", "OK");
        }
    }

    [RelayCommand] 
    private async Task Cancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}