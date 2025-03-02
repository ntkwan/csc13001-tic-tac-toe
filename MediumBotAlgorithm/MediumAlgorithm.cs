using System;
using CaroBotAlgorithm;

namespace MediumBotAlgorithm;

public class MediumAlgorithm : TicTacToeMinimaxBase
{
    private readonly double optimalMoveChance;

    public MediumAlgorithm() : this(0.51)
    {
    }
    public MediumAlgorithm(double optimalMoveChance)
    {
        if (optimalMoveChance < 0 || optimalMoveChance > 1)
            throw new ArgumentOutOfRangeException(nameof(optimalMoveChance), "Value must be between 0 and 1.");
        this.optimalMoveChance = optimalMoveChance;
    }

    public override (int row, int col) GetMove(char[,] board, char computerSymbol)
    {
        // Choose optimal move based on the configured chance; otherwise, pick a random move.
        if (random.NextDouble() < optimalMoveChance)
            return GetOptimalMove(board, computerSymbol);
        else
            return GetRandomMove(board);
    }
}
