
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProductCatalog.Maui.Messages;
using Resources.Shared.Interfaces;
using Resources.Shared.Models;

namespace ProductCatalog.Maui.ViewModels;

public partial class ProductListViewModel : ObservableObject, IRecipient<ProductUpdatedMessage>
{
    private readonly IProductService _productService;

    [ObservableProperty]
    private ObservableCollection<Product> _products = [];

    [ObservableProperty]
    private ObservableCollection<Category> _categories = [];

    [ObservableProperty] 
    private bool _isIdVisible;
    
    [ObservableProperty]
    private bool _isSidebarVisible = true;
    

    public ProductListViewModel(IProductService productService)
    {
        _productService = productService;
        WeakReferenceMessenger.Default.Register(this);
        LoadData();
    }
    
    private void LoadData()
    {
        LoadProducts();
        LoadCategories();
    }

    private void LoadProducts()
    {
        var result = _productService.GetAllProducts();
        if (result.Success)
        {
            if (result.Result != null) 
                Products = new ObservableCollection<Product>(result.Result);
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.DisplayAlert("Error", result.Message ?? "Failed to load products", "OK");
            });
        }
    }
    
    private void LoadCategories()
    {
        var result = _productService.GetAllCategories();
        if (result.Success && result.Result != null)
        {
            Categories = new ObservableCollection<Category>(result.Result);
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load categories", "OK");
            });
        }
    }
    
    // Got some help from gpt to implement IRecipient interface and Receive method to handle messages that update product data.
    public void Receive(ProductUpdatedMessage message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var updatedProduct = message.Value;
            var existingProduct = Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (existingProduct != null)
            {
                var index = Products.IndexOf(existingProduct);
                Products[index] = updatedProduct;
                OnPropertyChanged(nameof(Products));
                
                LoadCategories();
            }
        });
    }

    // Navigation
    [RelayCommand]
    private async Task NavigateToCreate()
    {
        await Shell.Current.GoToAsync("CreateProduct");
        Shell.Current.Navigated += Shell_Navigated!;
    }
    
    // Got some help from GPT to implement the Shell_Navigated method for managing data updates when navigating between pages/views.
    private void Shell_Navigated(object sender, ShellNavigatedEventArgs e)
    {
        if (e.Current.Location.OriginalString == "//ProductList")
        {
            LoadData();
            Shell.Current.Navigated -= Shell_Navigated!;
        }
    }

    [RelayCommand]
    private async Task NavigateToEdit(Product product)
    {
        await Shell.Current.GoToAsync("EditProduct", new Dictionary<string, object>
        {
            { "Product", product }
        });
    }

    [RelayCommand]
    private async Task DeleteProduct(Product product)
    {
        bool answer = await Shell.Current.DisplayAlert(
            "Delete Product",
            $"Are you sure you want to delete {product.Name}?",
            "Yes", "No");

        if (answer)
        {
            var result = _productService.RemoveProduct(product.Name);
            if (result.Success)
            {
                Products.Remove(product);
                LoadCategories();
                await Shell.Current.DisplayAlert("Success", result.Message, "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", result.Message ?? "Failed to delete product", "OK");
            }
        }
    }

    // Manage UI visibility states dynamically.
    [RelayCommand]
    private void ToggleIdVisibility() => IsIdVisible = !IsIdVisible;

    [RelayCommand]
    private void ToggleSidebar() => IsSidebarVisible = !IsSidebarVisible;
    
    ~ProductListViewModel() => Cleanup();

    public void Cleanup()
    {
        WeakReferenceMessenger.Default.Unregister<ProductUpdatedMessage>(this);
        Shell.Current.Navigated -= Shell_Navigated!;
    }
}