using Chess.Models;
using Xamarin.Forms;
using Chess.Utils;
using Cell = Chess.Models.Cell;
using Chess.Moves;
using Chess.Moves.Actions;

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

        private void AddAdditionalMoves(GameState game, Move move)
        {
            var setIsMovedAction = new SetIsMovedAction(move.FromCell, game);
            move.Actions.Insert(0, setIsMovedAction);

            if (game.Board[move.ToCell.Row, move.ToCell.Col].Player == Helpers.GetOpposingPlayer(game.CurrentPlayer))
            {
                var captureAction = new CapturePieceAction(new Cell(move.ToCell.Row, move.ToCell.Col), game);
                move.Actions.Insert(0, captureAction);
            }
            move.Actions.Add(new UpdateCurrentPlayerAction(game));
        }
    }
}
