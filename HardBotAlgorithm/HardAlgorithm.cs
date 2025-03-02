using System;
using System.Collections.Generic;
using CaroBotAlgorithm;

namespace HardBotAlgorithm;

public class HardAlgorithm : TicTacToeMinimaxBase
{
    public override (int row, int col) GetMove(char[,] board, char computerSymbol)
    {
        var moves = GetOptimalMove(board, computerSymbol); 
        moves.Sort((a, b) => b.score.CompareTo(a.score));
        return moves[0].move;
    }
}