
using ProductCatalog.Maui.ViewModels;

namespace ProductCatalog.Maui.Views;

public partial class ProductListView : ContentPage
{
    public ProductListView(ProductListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}