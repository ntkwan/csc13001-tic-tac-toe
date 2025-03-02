using System;
using System.Collections.Generic;
using System.Linq;
using CaroBotAlgorithm;

namespace MediumBotAlgorithm;

public class MediumAlgorithm : TicTacToeMinimaxBase
{
    private readonly double optimalMoveChance;

    public MediumAlgorithm() : this(0.51) // change the win ratio here
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
        var moves = GetOptimalMove(board, computerSymbol);
        var bestMove = moves.OrderByDescending(m => m.score).First().move;
        if (random.NextDouble() < optimalMoveChance)
        {
            return bestMove;
        }
        else
        {
            // Otherwise, choose an alternative move using a softmax over the remaining moves.
            // If no alternative exists, return the best move.
            var alternativeMoves = moves.Where(m => m.move != bestMove).ToList();
            if (alternativeMoves.Count == 0)
                return bestMove;

            // Temperature parameter to control the softness of the distribution.
            // Lower temperature makes the distribution peakier.
            double temperature = 1.0;

            // Compute softmax scores for alternative moves.
            var expScores = alternativeMoves.Select(m => Math.Exp(m.score / temperature)).ToList();
            double sumExp = expScores.Sum();
            double randVal = random.NextDouble() * sumExp;
            double cumulative = 0;

            for (int i = 0; i < alternativeMoves.Count; i++)
            {
                cumulative += expScores[i];
                if (randVal < cumulative)
                    return alternativeMoves[i].move;
            }
            return alternativeMoves.Last().move;
        }
    }
}
