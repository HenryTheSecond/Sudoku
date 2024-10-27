using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Sudoku.Models;

public class SudokuCell
{
    public int Value { get; set; }
    public bool IsEditable { get; set; }
    public Brush Background { get; set; } = new SolidColorBrush(Colors.White);
}

public class CellValueConverter : IValueConverter
{
    public int MinValue { get; } = 0;
    public int MaxValue { get; set; }
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || (int)value == 0)
        {
            return string.Empty;
        }

        return value.ToString()!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value == null || value.ToString() == string.Empty || (!int.TryParse(value.ToString(), out var num)) || num < MinValue || num > MaxValue)
        {
            return 0;
        }

        return num;
    }
}
