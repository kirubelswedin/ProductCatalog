
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ProductCatalog.Maui.Converters;

// Got help from gpt to implement this ScreenTriggerConverter for Sidebar and HamburgerMenu
public abstract partial class BaseScreenTriggerConverter : ObservableObject, IValueConverter
{
    public enum ScreenSize
    {
        Small = 600,
        Medium = 900,
        Large = 1200
    }

    [ObservableProperty]
    private double _threshold = (double)ScreenSize.Medium;

    [ObservableProperty]
    private ScreenSize _size = ScreenSize.Medium;

    partial void OnSizeChanged(ScreenSize value)
    {
        Threshold = (double)value;
    }

    partial void OnThresholdChanged(double value)
    {
        Size = (ScreenSize)value;
    }

    protected abstract bool EvaluateCondition(double value);

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double screenValue)
        {
            return EvaluateCondition(screenValue);
        }
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class GreaterThanScreenTrigger : BaseScreenTriggerConverter
{
    protected override bool EvaluateCondition(double value) => value > Threshold;
}

public class LessThanScreenTrigger : BaseScreenTriggerConverter
{
    protected override bool EvaluateCondition(double value) => value <= Threshold;
}

