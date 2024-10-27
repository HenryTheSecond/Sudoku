using CommunityToolkit.Mvvm.Messaging;
using Shared.Models.Responses;
using Sudoku.Interfaces.Services;
using Sudoku.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Sudoku.ViewModels;

public class SudokuHistoryViewModel : INotifyPropertyChanged
{
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<SudokuBoardHistory> PlayedGames { get; set; } = [];
    public SudokuBoardHistory? SelectedSudokuHistory { get; set; }
    public ObservableCollection<int> SelectedHistorySudokuBoard { get; set; } = [];
    public ICommand SelectedHistoryChangedCommand { get;}

    public SudokuHistoryViewModel(ISudokuService sudokuService)
    {
        GetSudokuHistory(sudokuService);
        SelectedHistoryChangedCommand = new RelayCommand(SelectedHistoryChanged);
        WeakReferenceMessenger.Default.Register<SudokuSolveNotification>(this, (r, m) => GetSudokuHistory(sudokuService));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private async void GetSudokuHistory(ISudokuService sudokuService)
    {
        try
        {
            IsBusy = true;
            PlayedGames = new(await sudokuService.GetSudokuBoardHistory());
            OnPropertyChanged(nameof(PlayedGames));
        }
        catch
        {
            MessageBox.Show("Couldn't load history from database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void SelectedHistoryChanged()
    {
        SelectedHistorySudokuBoard.Clear();
        if(SelectedSudokuHistory == null)
        {
            return;
        }
        foreach(var num in SelectedSudokuHistory.Board.SelectMany(x => x))
        {
            SelectedHistorySudokuBoard.Add(num);
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
