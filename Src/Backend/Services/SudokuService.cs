using Backend.Interfaces.Repositories;
using Backend.Interfaces.Services;
using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Requests;
using Shared.Models.Responses;

namespace Backend.Services
{
    public class SudokuService(ISudokuRepository sudokuRepository) : ISudokuService
    {
        public async Task AddSudokuBoard(SudokuBoardRequest request)
        {
            sudokuRepository.Add(new SudokuBoard
            {
                Board = string.Join(',', request.Board),
                SolvedTime = DateTime.Now
            });

            await sudokuRepository.SaveChangeAsync();
        }

        public async Task<List<SudokuBoardHistory>> GetSudokuBoardHistory()
        {
            return (await sudokuRepository.Query.OrderByDescending(x => x.SolvedTime).ToListAsync())
                .Select(x => new SudokuBoardHistory
                {
                    Id = x.Id,
                    Board = ConvertStringToSudokuBoard(x.Board),
                    SolvedTime = x.SolvedTime
                }).ToList();
        }

        private static int[][] ConvertStringToSudokuBoard(string str)
        {
            var nums = str.Split(',').Select(int.Parse).ToList();
            int boardSize = (int)Math.Sqrt(nums.Count);
            int[][] board = new int[boardSize][];
            for(int i = 0; i < boardSize; i++)
            {
                board[i] = new int[boardSize];
                for(int j = 0; j < boardSize; j++)
                {
                    board[i][j] = nums[i * boardSize + j];
                }
            }
            return board;
        }
    }
}
