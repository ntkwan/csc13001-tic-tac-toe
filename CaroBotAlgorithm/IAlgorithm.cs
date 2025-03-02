// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using System.Collections.Generic;
using System;

namespace CaroBotAlgorithm;

public interface IAlgorithm
{
    (int row, int col) GetMove(char[,] board, char computerSymbol);
}

public abstract class TicTacToeMinimaxBase : IAlgorithm
{
    protected readonly Random random = new();

    // Abstract interface method that derived classes must implement.
    public abstract (int row, int col) GetMove(char[,] board, char computerSymbol);

    // Returns the optimal move using the minimax algorithm with alpha–beta pruning.
    protected (int, int) GetOptimalMove(char[,] board, char computerSymbol)
    {
        char opponent = computerSymbol == 'X' ? 'O' : 'X';
        int bestVal = int.MinValue;
        (int, int) bestMove = (-1, -1);
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (board[i, j] == ' ')
                {
                    board[i, j] = computerSymbol;
                    int moveVal = Minimax(board, 0, false, int.MinValue, int.MaxValue, computerSymbol, opponent);
                    board[i, j] = ' ';

                    if (moveVal > bestVal)
                    {
                        bestVal = moveVal;
                        bestMove = (i, j);
                    }
                }
            }
        }
        return bestMove;
    }

    // Minimax algorithm with alpha-beta pruning.
    protected int Minimax(char[,] board, int depth, bool isMax, int alpha, int beta, char computerSymbol, char opponent)
    {
        int score = Evaluate(board, computerSymbol, opponent);

        // Terminal states: return score adjusted by depth.
        if (score == 10)
            return score - depth;
        if (score == -10)
            return score + depth;
        if (!IsMovesLeft(board))
            return 0;

        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        if (isMax)
        {
            int best = int.MinValue;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = computerSymbol;
                        best = Math.Max(best, Minimax(board, depth + 1, false, alpha, beta, computerSymbol, opponent));
                        board[i, j] = ' ';
                        alpha = Math.Max(alpha, best);
                        if (beta <= alpha)
                            break;
                    }
                }
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = opponent;
                        best = Math.Min(best, Minimax(board, depth + 1, true, alpha, beta, computerSymbol, opponent));
                        board[i, j] = ' ';
                        beta = Math.Min(beta, best);
                        if (beta <= alpha)
                            break;
                    }
                }
            }
            return best;
        }
    }

    // Evaluation function: returns +10 for a win for computer, -10 for a win for opponent, or 0 otherwise.
    protected int Evaluate(char[,] board, char computerSymbol, char opponent)
    {
        int n = board.GetLength(0);

        // Check rows.
        for (int i = 0; i < n; i++)
        {
            if (board[i, 0] == board[i, 1] &&
                board[i, 1] == board[i, 2] &&
                board[i, 0] != ' ')
            {
                if (board[i, 0] == computerSymbol)
                    return 10;
                else if (board[i, 0] == opponent)
                    return -10;
            }
        }
        // Check columns.
        for (int j = 0; j < n; j++)
        {
            if (board[0, j] == board[1, j] &&
                board[1, j] == board[2, j] &&
                board[0, j] != ' ')
            {
                if (board[0, j] == computerSymbol)
                    return 10;
                else if (board[0, j] == opponent)
                    return -10;
            }
        }
        // Check diagonals.
        if (board[0, 0] == board[1, 1] &&
            board[1, 1] == board[2, 2] &&
            board[0, 0] != ' ')
        {
            if (board[0, 0] == computerSymbol)
                return 10;
            else if (board[0, 0] == opponent)
                return -10;
        }
        if (board[0, 2] == board[1, 1] &&
            board[1, 1] == board[2, 0] &&
            board[0, 2] != ' ')
        {
            if (board[0, 2] == computerSymbol)
                return 10;
            else if (board[0, 2] == opponent)
                return -10;
        }
        return 0;
    }

    // Returns true if there are moves left on the board.
    protected bool IsMovesLeft(char[,] board)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                if (board[i, j] == ' ')
                    return true;
        return false;
    }

    // Returns a random move from the available moves.
    protected (int, int) GetRandomMove(char[,] board)
    {
        List<(int, int)> availableMoves = new List<(int, int)>();
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (board[i, j] == ' ')
                    availableMoves.Add((i, j));
            }
        }
        if (availableMoves.Count == 0)
            return (-1, -1);

        int index = random.Next(availableMoves.Count);
        return availableMoves[index];
    }
}