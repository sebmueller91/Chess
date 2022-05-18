using Chess.AI.Interfaces;
using Chess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.AI
{
    internal class EvaluationFunctionService : IEvaluationFunctionService
    {
        public int SimpleEvaluation(GameState game, Player player)
        {
            return 0;
        }
    }
}
