using Backend.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Requests;

namespace Backend.Controllers
{
    [Route("sudoku")]
    [ApiController]
    public class SudokuController(ISudokuService sudokuService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> AddSudokuController(SudokuBoardRequest request)
        {
            await sudokuService.AddSudokuBoard(request);
            return Ok();
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetSudokuBoardHistory()
        {
            return Ok(await sudokuService.GetSudokuBoardHistory());
        }
    }
}
