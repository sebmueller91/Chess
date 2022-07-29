using Chess.Models;
using Chess.Models.Pieces;
using Chess.Moves;
using Chess.Services;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace Chess.AI
{
    public class EvaluationFunction
    {
        private readonly IPieceMoveOptionsService _pieceMoveOptionsService;
        private readonly EvaluationWeights _evaluationWeights;

        private readonly GameState _game;
        private readonly Player _aiPlayer;

        public EvaluationFunction(Difficulty difficulty, GameState game, Player aiPlayer)
        {
            _pieceMoveOptionsService = App.UnityContainer.Resolve<IPieceMoveOptionsService>();
            _game = game;
            _aiPlayer = aiPlayer;
            _evaluationWeights = EvaluationWeightsFactory.CreateWeights(game, difficulty);
        }


        public int EvaluateGameState()
        {
            var score = 0;

            if (_pieceMoveOptionsService.IsStalemate(_game, _game.CurrentPlayer)) {
                return 0;
            }

            if (_pieceMoveOptionsService.IsCheckmate(_game, _game.CurrentPlayer))
            {
                return (_game.CurrentPlayer == _aiPlayer) ? Int32.MinValue : Int32.MaxValue;
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (_game.Board[i][j] is Empty)
                    {
                        continue;
                    }

                    var sign = _game.Board[i][j].Player == _aiPlayer ? 1 : -1;
                    var type = _game.Board[i][j].GetType();
                    var squareTable = _evaluationWeights.GetSquareTableForPieceType(type);

                    var sqTableIndex = (_aiPlayer == Player.White) ? i : 7 - i;
                    var currentScore = sign * (_evaluationWeights.PieceWeights[type] + squareTable[sqTableIndex, j]);

                    score += currentScore;
                }
            }

            return score;
        }
    }
}
