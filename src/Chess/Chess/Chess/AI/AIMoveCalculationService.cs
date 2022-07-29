using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Config;
using Chess.Models;
using Chess.Moves;
using Chess.Services;
using Chess.Utils;
using Unity;

namespace Chess.AI
{
    public class AIMoveCalculationService
    {
        private readonly EvaluationFunction _evaluationFunction;
        private readonly IExecutePieceMoveService _executePieceMoveService;
        private readonly IPieceMoveOptionsService _pieceMoveOptionsService;

        private GameState _game;
        private Player _aiPlayer;

        private const int MAX_DEPTH = 2; // TODO: Move into constants? Or Is this part of the difficulty?

        public AIMoveCalculationService(Difficulty difficulty, GameState game, Player aiPlayer)
        {
            _executePieceMoveService = App.UnityContainer.Resolve<IExecutePieceMoveService>();
            _pieceMoveOptionsService = App.UnityContainer.Resolve<IPieceMoveOptionsService>();
            _evaluationFunction = new EvaluationFunction(difficulty, game, aiPlayer);

            _game = game;
            _aiPlayer = aiPlayer;
        }

        public Move GetNextMoveForPlayer()
        {
            var moves = new List<Move>();
            var bestMoveSeries = MinMaxWithAlphaBetaPruning(moves, 0, Int32.MinValue, Int32.MaxValue);
            return bestMoveSeries.Moves[0];
        }

        private MoveSeries MinMaxWithAlphaBetaPruning(List<Move> moves, int depth, int alpha, int beta)
        {    
            if (depth == MAX_DEPTH)
            {
                return EvaluateMoves(moves);
            }
     
            var isCheckMate = _pieceMoveOptionsService.IsCheckmate(_game, _game.CurrentPlayer);
            var isStaleMate = _pieceMoveOptionsService.IsStalemate(_game, _game.CurrentPlayer);

            if (isCheckMate || isStaleMate) {
                return EvaluateMoves(moves);
            }
            else
            {
                var possibleMoves = GetAllPossibleMovesOfCurrentPlayer();
                if (_game.CurrentPlayer == _aiPlayer) // Maximize
                {
                    MoveSeries bestSeries = null;
                    foreach (var curMove in possibleMoves)
                    {
                        _executePieceMoveService.AddAdditionalMoves(_game, curMove); // TODO: Is this needed?
                        moves.Add(curMove);
                        var oldMoveStack = _game.MoveStack.Clone();

                        if (curMove.FromCell.Equals(new Cell(6,0)) && curMove.ToCell.Equals(new Cell(4,0)))
                        {
                            var a = 4;
                        }
                        _executePieceMoveService.ExecuteMove(_game, curMove);
                        var curSeries = MinMaxWithAlphaBetaPruning(CloneList(moves), depth + 1, alpha, beta);

                        if (bestSeries == null || curSeries.Score > bestSeries.Score)
                        {
                            bestSeries = curSeries.Clone();
                        }
                        RollbackAndRemoveLastMove(moves, oldMoveStack);

                        alpha = Max(curSeries.Score, alpha);
                        if (bestSeries.Score >= beta)
                        {
                            break;
                        }
                    }
                    return bestSeries;
                }
                else // Minimize
                {
                    MoveSeries bestSeries = null;
                    foreach (var curMove in possibleMoves)
                    {
                        moves.Add(curMove);
                        var oldMoveStack = _game.MoveStack.Clone();

                        _executePieceMoveService.ExecuteMove(_game, moves.Last());
                        var curSeries = MinMaxWithAlphaBetaPruning(CloneList(moves), depth + 1, alpha, beta);

                        if (bestSeries == null || curSeries.Score < bestSeries.Score)
                        {
                            bestSeries = curSeries.Clone();

                        }

                        RollbackAndRemoveLastMove(moves, oldMoveStack);

                        beta = Min(curSeries.Score, alpha);
                        if (bestSeries.Score <= alpha)
                        {
                            break;
                        }
                    }

                    return bestSeries;
                }
            }
        }

        private void RollbackAndRemoveLastMove(List<Move> moves, MoveStack oldMoveStack)
        {
            _executePieceMoveService.RollbackLastMove(_game);
            moves.Remove(moves.Last());
            Helpers.GetCurrentGame().MoveStack = oldMoveStack;
        }

        private List<Move> CloneList(List<Move> list)
        {
            var newList = new List<Move>();
            for (int i = 0; i < list.Count; i++)
            {
                newList.Add(list[i].Clone());
            }
            return newList;
        }

        private MoveSeries EvaluateMoves(List<Move> moves)
        {
            var score = _evaluationFunction.EvaluateGameState();
            return new MoveSeries(moves, score);
        }

        private List<Move> GetAllPossibleMovesOfCurrentPlayer()
        {
            var moves = new List<Move>();
            var pieces = Helpers.GetAllPicesOfPlayer(_game, _game.CurrentPlayer);

            foreach (var piece in pieces)
            {
                if (piece.Player == _game.CurrentPlayer)
                {
                    moves.AddRange(_pieceMoveOptionsService.GetPossibleMoves(_game, piece.Position));
                }
            }

            return moves;
        }

        private int Max(int i1, int i2)
        {
            return (i1 > i2) ? i1 : i2;
        }

        private int Min(int i1, int i2)
        {
            return (i1 < i2) ? i1 : i2;
        }

        private class MoveSeries 
        {
            public List<Move> Moves { get; set; }
            public int Score { get; set; }
            public MoveSeries(List<Move> moves, int score)
            {
                Moves = moves;
                Score = score;
            }

            public MoveSeries Clone()
            {
                var clonedMoves = new List<Move>();
                for (int i = 0; i < this.Moves.Count; i++)
                {
                    clonedMoves.Add(this.Moves[i].Clone());
                }

                return new MoveSeries(clonedMoves, this.Score);
            }
        }
    }
}
