// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using System;
using System.Collections.Generic;
using CaroBotAlgorithm;

namespace EasyBotAlgorithm;

public class EasyAlgorithm : IAlgorithm
{
    private readonly Random random = new();

    public (int row, int col) GetMove(char[,] board, char computerSymbol)
    {
        List<(int, int)> availableMoves = new List<(int, int)>();
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        // Tìm các ô trống trên bàn cờ
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

        // Chọn ngẫu nhiên 1 nước đi trong số các ô trống
        int index = random.Next(availableMoves.Count);
        return availableMoves[index];
    }
}
