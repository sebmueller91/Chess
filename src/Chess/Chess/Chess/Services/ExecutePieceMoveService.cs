using Chess.Models;
using Xamarin.Forms;
using Chess.Utils;
using Cell = Chess.Models.Cell;
using Chess.Moves;
using Chess.Moves.Actions;
using System;
using System.Linq;

namespace Chess.Services
{
    public class ExecutePieceMoveService : IExecutePieceMoveService
    {

        public ExecutePieceMoveService()
        {
        }

        public void ExecuteMove(GameState game, Move move)
        {
            AddAdditionalMoves(game, move);
            game.MoveStack.ExecuteMove(move);
        }

        public void RollbackLastMove(GameState game)
        {
            game.MoveStack.RollbackLastMove();
        }

        public void RollbackSimulatedMove(Move move)
        {
            move.Rollback();
        }

        public void SimulateExecuteMove(GameState game, Move move)
        {
            AddAdditionalMoves(game, move);
            move.Execute();
        }

        public void AddAdditionalMoves(GameState game, Move move) 
        {
            AddActionToMoveIfNoActionOfSameTypeContained(move, new SetIsMovedAction(move.FromCell), true);

            if (game.Board[move.ToCell.Row][move.ToCell.Col].Player == Helpers.GetOpposingPlayer(game.CurrentPlayer))
            {
                var captureAction = new CapturePieceAction(move.ToCell);
                AddActionToMoveIfNoActionOfSameTypeContained(move, captureAction, true);
            }
            AddActionToMoveIfNoActionOfSameTypeContained(move, new UpdateCurrentPlayerAction());
        }

        private void AddActionToMoveIfNoActionOfSameTypeContained(Move move, RevertableAction action, bool prepend = false)
        {
            if (MoveContainsActionOfType(move, action.GetType()))
            {
                return;
            }

            if (prepend)
            {
                move?.Actions.Insert(0, action);
            } else
            {
                move.Actions.Add(action);
            }
        }

        private bool MoveContainsActionOfType(Move move, Type type)
        {
            for(int i = 0; i < move?.Actions.Count; i++)
            {
                var action = move.Actions[i];
                if (action.GetType() == type)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
