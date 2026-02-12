using System.Globalization;

namespace WarehouseStockOpname.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status switch
            {
                "Draft" => Color.FromArgb("#FFA500"),
                "InProgress" => Color.FromArgb("#0066CC"),
                "Completed" => Color.FromArgb("#008000"),
                _ => Color.FromArgb("#808080")
            };
        }
        return Color.FromArgb("#808080");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
