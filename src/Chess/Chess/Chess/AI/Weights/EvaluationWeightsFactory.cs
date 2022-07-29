using Chess.AI.Weights;
using Chess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.AI
{
    internal class EvaluationWeightsFactory
    {
        public static EvaluationWeights CreateWeights(GameState game, Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return new EvaluationWeightsEasy(game);
                case Difficulty.Normal:
                    return new EvaluationWeightsNormal(game);
                default:
                    return new EvaluationWeightsHard(game);
            }
        }
    }
}
