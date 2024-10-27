using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Sudoku.Models;

public class SudokuCell(int value, bool isReadOnly, Brush background)
{
    public static readonly Brush EditableBackground = new SolidColorBrush(Colors.White);
    public static readonly Brush UneditableBackground = new SolidColorBrush(Colors.Gray);
    public static readonly Brush HintBackground = new SolidColorBrush(Colors.Yellow);
    public int Value { get; set; } = value;
    public bool IsReadOnly { get; set; } = isReadOnly;
    public Brush Background { get; set; } = background;
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
