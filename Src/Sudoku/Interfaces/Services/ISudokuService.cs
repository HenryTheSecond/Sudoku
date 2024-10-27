using Shared.Models.Requests;
using Shared.Models.Responses;

namespace Sudoku.Interfaces.Services;

public interface ISudokuService
{
    Task<bool> AddSudokuBoard(SudokuBoardRequest request);
    Task<List<SudokuBoardHistory>> GetSudokuBoardHistory();
}