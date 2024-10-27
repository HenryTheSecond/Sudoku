namespace Sudoku.Models;

public class SudokuSolver
{
    private int[,] board;
    public int Size { get; }
    public int BoardSize => Size * Size;
    public SudokuSolver(int size)
    {
        Size = size;
        board = new int[BoardSize, BoardSize];
        GenerateSolvedBoard();
    }

    private void GenerateSolvedBoard()
    {
        var random = new Random();

        bool[,] rowOccupiedNumbers = new bool[BoardSize, BoardSize];
        bool[,] colOccupiedNumbers = new bool[BoardSize, BoardSize];
        bool[,] groupOccupiedNumbers = new bool[BoardSize, BoardSize];

        GenerateBacktracking(0, 0);

        bool GenerateBacktracking(int row, int col)
        {
            if (row == BoardSize)
            {
                return true;
            }
            int group = Size * (row / Size) + (col / Size);

            var nums = Enumerable.Range(0, BoardSize).ToArray();
            random.Shuffle(nums);
            foreach (var i in nums)
            {
                if (!rowOccupiedNumbers[row, i] && !colOccupiedNumbers[col, i] && !groupOccupiedNumbers[group, i])
                {
                    rowOccupiedNumbers[row, i] = true;
                    colOccupiedNumbers[col, i] = true;
                    groupOccupiedNumbers[group, i] = true;

                    board[row, col] = i + 1;
                    var nextRow = row;
                    var nextCol = col + 1;
                    if (nextCol == BoardSize)
                    {
                        nextRow++;
                        nextCol = 0;
                    }
                    var nextStep = GenerateBacktracking(nextRow, nextCol);
                    if (nextStep == true)
                    {
                        return true;
                    }

                    rowOccupiedNumbers[row, i] = false;
                    colOccupiedNumbers[col, i] = false;
                    groupOccupiedNumbers[group, i] = false;
                }
            }

            return false;
        }
    }

    public int GetValue(int row, int col)
    {
        return board[row, col];
    }

    public int[,] GenerateUniqueSolutionPlayableBoard()
    {
        int[] cells = Enumerable.Range(0, BoardSize * BoardSize).ToArray();

        // Random the cell's order before removing
        var random = new Random();
        random.Shuffle(cells);

        int[,] playableBoard = new int[BoardSize, BoardSize];

        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                playableBoard[i, j] = board[i, j];
            }
        }

        for (int i = 0; i < BoardSize * BoardSize; i++)
        {
            int row = cells[i] / BoardSize;
            int col = cells[i] % BoardSize;
            var tmp = playableBoard[row, col];
            playableBoard[row, col] = 0;

            // Try to remove each cell then check if we still have unique solution. Otherwise, don't pick the cell
            if (!CheckUniqueSolution(playableBoard))
            {
                playableBoard[row, col] = tmp;
            }
        }

        return playableBoard;
    }

    private static bool CheckUniqueSolution(int[,] board)
    {
        int boardSize = board.GetLength(0);
        int size = (int)Math.Sqrt(boardSize);
        bool[,] rowOccupiedNumbers = new bool[boardSize, boardSize];
        bool[,] colOccupiedNumbers = new bool[boardSize, boardSize];
        bool[,] groupOccupiedNumbers = new bool[boardSize, boardSize];
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                if (board[row, col] != 0)
                {
                    int group = size * (row / size) + (col / size);
                    rowOccupiedNumbers[row, board[row, col] - 1] = true;
                    colOccupiedNumbers[col, board[row, col] - 1] = true;
                    groupOccupiedNumbers[group, board[row, col] - 1] = true;
                }
            }
        }

        int countSolution = 0;
        (var initRow, var initCol) = FindMostConstrainedCell(board, rowOccupiedNumbers, colOccupiedNumbers, groupOccupiedNumbers);
        FindSolution(initRow, initCol);
        return countSolution == 1;

        void FindSolution(int row, int col)
        {
            if (row == boardSize)
            {
                countSolution++;
                return;
            }

            int group = size * (row / size) + (col / size);

            for (int i = 0; i < boardSize; i++)
            {
                if (!rowOccupiedNumbers[row, i] && !colOccupiedNumbers[col, i] && !groupOccupiedNumbers[group, i])
                {
                    rowOccupiedNumbers[row, i] = true;
                    colOccupiedNumbers[col, i] = true;
                    groupOccupiedNumbers[group, i] = true;
                    board[row, col] = i + 1;

                    (var nextRow, var nextCol) = FindMostConstrainedCell(board, rowOccupiedNumbers, colOccupiedNumbers, groupOccupiedNumbers);
                    FindSolution(nextRow, nextCol);

                    rowOccupiedNumbers[row, i] = false;
                    colOccupiedNumbers[col, i] = false;
                    groupOccupiedNumbers[group, i] = false;
                    board[row, col] = 0;

                    if (countSolution > 1)
                    {
                        break;
                    }
                }
            }
        }
    }

    private static (int Row, int Col) FindMostConstrainedCell(int[,] board, bool[,] rowOccupiedNumbers, bool[,] colOccupiedNumbers, bool[,] groupOccupiedNumbers)
    {
        int boardSize = board.GetLength(0);
        int size = (int)Math.Sqrt(boardSize);

        int min = boardSize + 1;
        int row = -1;
        int col = -1;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (board[i, j] != 0)
                {
                    continue;
                }
                int group = (i / size) * size + (j / size);
                int count = 0;
                for (int n = 0; n < boardSize; n++)
                {
                    if (!rowOccupiedNumbers[i, n] && !colOccupiedNumbers[j, n] && !groupOccupiedNumbers[group, n])
                    {
                        count++;
                    }
                }

                if (count < min)
                {
                    min = count;
                    row = i;
                    col = j;
                }
            }
        }

        if (row != -1 && col != -1)
        {
            return (row, col);
        }

        return (boardSize, boardSize);
    }
}
