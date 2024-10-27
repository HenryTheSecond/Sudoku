namespace Backend.Models.Entities;

public class SudokuBoard
{
    public long Id { get; set; }
    public string Board { get; set; } = string.Empty;
    public DateTime SolvedTime { get; set; }
}
