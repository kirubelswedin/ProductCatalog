
using ProductCatalog.Maui.ViewModels;

namespace ProductCatalog.Maui.Views;

public partial class CreateProductView : ContentPage
{
    public CreateProductView(CreateProductViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}