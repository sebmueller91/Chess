using Chess.Models;
using Chess.Moves;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Services
{
    public interface IPieceMoveOptionsService
    {
        bool KingIsInCheck(GameState game, Player playerToCheck);
        List<Move> GetPossibleMoves(GameState game, Cell position);
        bool IsCheckmate(GameState game, Player playerToCheck);
        bool IsStalemate(GameState game, Player playerToCheck);
    }
}
