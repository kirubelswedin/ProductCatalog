
namespace ProductCatalog.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Current!.UserAppTheme = AppTheme.Light;
        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.MinimumHeight = 900;
        window.MinimumWidth = 600;
        return window;
    }
}