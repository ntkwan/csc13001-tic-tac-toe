using System;
using CaroBotAlgorithm;

namespace HardBotAlgorithm;

public class HardAlgorithm : TicTacToeMinimaxBase
{
    public override (int row, int col) GetMove(char[,] board, char computerSymbol)
    {
        return GetOptimalMove(board, computerSymbol);   
    }
}