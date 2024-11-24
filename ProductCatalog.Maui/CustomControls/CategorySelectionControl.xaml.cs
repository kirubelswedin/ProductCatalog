
namespace ProductCatalog.Maui.CustomControls;

public partial class CategorySelectionControl : ContentView
{
    public CategorySelectionControl()
    {
        InitializeComponent();
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        CategoryExpander.IsExpanded = false;
    }
}