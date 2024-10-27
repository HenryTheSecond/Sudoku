using CommunityToolkit.Mvvm.Messaging;
using Shared.Models.Requests;
using Sudoku.Interfaces.Services;
using Sudoku.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Sudoku.ViewModels;

public class SudokuViewModel : INotifyPropertyChanged
{
    private readonly ISudokuService sudokuService;
    private SudokuSolver solver;

    public event PropertyChangedEventHandler? PropertyChanged;

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

    public int Size { get; }
    public int BoardSize => Size * Size;
    public ObservableCollection<SudokuCell> Board { get; set; } = new();
    public ICommand SolveCommand { get; }
    public ICommand NewGameCommand { get; }
    public ICommand HintCommand { get; }
    public SudokuViewModel(int size, ISudokuService sudokuService)
    {
        this.sudokuService = sudokuService;
        Size = size;
        InitializeBoard();
        NewGameCommand = new RelayCommand(InitializeBoard);
        HintCommand = new RelayCommand(ShowHint);
        SolveCommand = new AsyncRelayCommand(SolveBoard);
    }

    private void InitializeBoard()
    {
        solver = new SudokuSolver(Size);
        var board = solver.GenerateUniqueSolutionPlayableBoard();
        Board.Clear();
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                Board.Add(new SudokuCell(board[i, j], 
                    board[i, j] != 0, 
                    board[i, j] == 0 ? SudokuCell.EditableBackground : SudokuCell.UneditableBackground));
            }
        }
    }

    private async Task SolveBoard()
    {
        // Check if all cells have the same value as solved sudoku
        // If not, we response an error message
        for(int i = 0; i < Board.Count; i++)
        {
            int row = i / BoardSize;
            int col = i % BoardSize;
            if (Board[i].Value != 0 && Board[i].Value != solver.GetValue(row, col))
            {
                MessageBox.Show("The board is unsolveable", "Unsolveable", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        // Solve the sudoku board if all the cells match the target values
        for(int i = 0; i < Board.Count; i++)
        {
            if (!Board[i].IsReadOnly)
            {
                int row = i / BoardSize;
                int col = i % BoardSize;
                Board[i] = new SudokuCell(solver.GetValue(row, col), true, SudokuCell.EditableBackground);
            }
        }
        await SaveSolvedSudokuToDatabase();
    }

    private async Task SaveSolvedSudokuToDatabase()
    {
        try
        {
            IsBusy = true;
            var result = await sudokuService.AddSudokuBoard(new SudokuBoardRequest { Board = Board.Select(x => x.Value).ToArray() });
            if (result)
            {
                MessageBox.Show("Saved to database", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                WeakReferenceMessenger.Default.Send(new SudokuSolveNotification());
            }
            else
            {
                MessageBox.Show("Couldn't save to database", "Unsaved", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        catch
        {
            MessageBox.Show("Couldn't save to database", "Unsaved", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ShowHint()
    {
        // We prioritize in finding and adjusting incorrect cell than fulfilling empty cell
        int incorrectCell = -1;
        int emptyCell = -1;
        for(int i = 0; i < Board.Count; i++)
        {
            int row = i / BoardSize;
            int col = i % BoardSize;
            if (Board[i].Value == 0)
            {
                if(emptyCell == -1)
                {
                    emptyCell = i;
                }
            }
            else if (Board[i].Value != solver.GetValue(row, col))
            {
                incorrectCell = i;
                break;
            }
        }

        if(incorrectCell != -1 || emptyCell != -1)
        {
            var modifiedCell = incorrectCell != -1 ? incorrectCell : emptyCell;
            int row = modifiedCell / BoardSize;
            int col = modifiedCell % BoardSize;
            Board[modifiedCell] = new SudokuCell(solver.GetValue(row, col), false, SudokuCell.HintBackground);
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
