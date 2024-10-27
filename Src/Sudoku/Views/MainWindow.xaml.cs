using Sudoku.Interfaces.Services;
using Sudoku.ViewModels;
using System.Windows;

namespace Sudoku;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel
        {
            SudokuViewModel = new SudokuViewModel(3, NinjectKernel.Get<ISudokuService>()),
            SudokuHistoryViewModel = NinjectKernel.Get<SudokuHistoryViewModel>()
        };
    }
}