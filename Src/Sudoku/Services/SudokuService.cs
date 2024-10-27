using Shared.Models.Requests;
using Shared.Models.Responses;
using Sudoku.Interfaces.Services;
using System.Net.Http;
using System.Net.Http.Json;

namespace Sudoku.Services;

public class SudokuService(HttpClient httpClient) : ISudokuService
{
    public async Task<bool> AddSudokuBoard(SudokuBoardRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/sudoku/add", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<SudokuBoardHistory>> GetSudokuBoardHistory()
    {
        return await httpClient.GetFromJsonAsync<List<SudokuBoardHistory>>("/sudoku/history")
            ?? throw new Exception();
    }
}
