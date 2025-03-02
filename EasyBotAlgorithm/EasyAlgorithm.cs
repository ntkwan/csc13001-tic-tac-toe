// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using System;
using System.Collections.Generic;
using CaroBotAlgorithm;

namespace EasyBotAlgorithm;

public class EasyAlgorithm : TicTacToeMinimaxBase
{
    public override (int row, int col) GetMove(char[,] board, char computerSymbol)
    {
        return GetRandomMove(board);
    }
}
