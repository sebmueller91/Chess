using Chess.AI.Weights;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.AI
{
    internal class WeightsFactory
    {
        public static EvaluationWeights CreateWeights(string difficulty)
        {
            switch (difficulty.ToLower())
            {
                case "easy":
                    return new EvaluationWeightsEasy();
                case "normal":
                    return new EvaluationWeightsNormal();
                    break;
                case "hard":
                    // TODO
                    break;
            }
            return null;
        }
    }
}
