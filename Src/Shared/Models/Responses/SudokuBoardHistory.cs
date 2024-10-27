namespace Shared.Models.Responses;

public class SudokuBoardHistory
{
    public long Id { get; set; }
    public int[][] Board { get; set; } = [];
    public DateTime SolvedTime { get; set; }
}
