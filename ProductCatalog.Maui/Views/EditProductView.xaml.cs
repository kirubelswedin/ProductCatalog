
using ProductCatalog.Maui.ViewModels;

namespace ProductCatalog.Maui.Views;

public partial class EditProductView : ContentPage
{
    public EditProductView(EditProductViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}