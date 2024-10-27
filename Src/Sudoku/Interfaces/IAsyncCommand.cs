using System.Windows.Input;

namespace Sudoku.Interfaces;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync(object? parameter);
}
