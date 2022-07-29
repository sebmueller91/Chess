using Chess.Models;
using Chess.Moves;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Services
{
    public interface IExecutePieceMoveService
    {
        void ExecuteMove(GameState game, Move move);
        void RollbackLastMove(GameState game);

        void SimulateExecuteMove(GameState game, Move move);
        void RollbackSimulatedMove(Move move);
        void AddAdditionalMoves(GameState game, Move move);
    }
}
