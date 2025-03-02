using System;
using CaroBotAlgorithm;

namespace HardBotAlgorithm;

public class HardAlgorithm : IAlgorithm
{
    // Returns the best move as a tuple (row, col)
    public (int row, int col) GetMove(char[,] board, char computerSymbol)
    {
        char opponent = computerSymbol == 'X' ? 'O' : 'X';
        int bestVal = int.MinValue;
        (int row, int col) bestMove = (-1, -1);
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        // Evaluate all possible moves
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (board[i, j] == ' ')
                {
                    // Make the move
                    board[i, j] = computerSymbol;
                    // Evaluate this move with minimax
                    int moveVal = Minimax(board, 0, false, int.MinValue, int.MaxValue, computerSymbol, opponent);
                    // Undo the move
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

    // Minimax algorithm with alpha-beta pruning
    private int Minimax(char[,] board, int depth, bool isMax, int alpha, int beta, char computerSymbol, char opponent)
    {
        int score = Evaluate(board, computerSymbol, opponent);

        // If terminal state, return evaluated score adjusted by depth
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

    // Evaluation function:
    // +10 if computer wins, -10 if opponent wins, 0 otherwise.
    private int Evaluate(char[,] board, char computerSymbol, char opponent)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        // Check rows for victory.
        for (int i = 0; i < rows; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != ' ')
            {
                if (board[i, 0] == computerSymbol)
                    return 10;
                else if (board[i, 0] == opponent)
                    return -10;
            }
        }
        // Check columns for victory.
        for (int j = 0; j < cols; j++)
        {
            if (board[0, j] == board[1, j] && board[1, j] == board[2, j] && board[0, j] != ' ')
            {
                if (board[0, j] == computerSymbol)
                    return 10;
                else if (board[0, j] == opponent)
                    return -10;
            }
        }
        // Check diagonals for victory.
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != ' ')
        {
            if (board[0, 0] == computerSymbol)
                return 10;
            else if (board[0, 0] == opponent)
                return -10;
        }
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != ' ')
        {
            if (board[0, 2] == computerSymbol)
                return 10;
            else if (board[0, 2] == opponent)
                return -10;
        }
        return 0;
    }

    // Check if there are any moves left on the board.
    private bool IsMovesLeft(char[,] board)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (board[i, j] == ' ')
                    return true;
            }
        }
        return false;
    }
}