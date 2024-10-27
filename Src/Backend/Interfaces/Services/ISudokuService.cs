using Shared.Models.Requests;
using Shared.Models.Responses;

namespace Backend.Interfaces.Services;

public interface ISudokuService
{
    Task AddSudokuBoard(SudokuBoardRequest request);
    Task<List<SudokuBoardHistory>> GetSudokuBoardHistory();
}
