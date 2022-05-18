using Chess.Models;
using Chess.Moves;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Services
{
    internal class PieceMoveOptionsService : IPieceMoveOptionsService
    {
        private readonly IExecutePieceMoveService _executePieceMoveService;
        public PieceMoveOptionsService(IExecutePieceMoveService executePieceMoveService)
        {
            _executePieceMoveService = executePieceMoveService;
        }

        public bool IsCheckmate(GameState game, Player playerToCheck) { 
            if (!KingIsInCheck(game, playerToCheck)) return false;           
            
            return PlayerCanPerformMove(game, playerToCheck);
        }

        public bool IsStalemate(GameState game, Player playerToCheck)
        {
            if (KingIsInCheck(game, playerToCheck)) return false;

            return PlayerCanPerformMove(game, playerToCheck);
        }

        public bool KingIsInCheck(GameState game, Player playerToCheck)
        {
            var cellsInCheck = PieceMoveHelpers.GetFieldsInCheck(game, playerToCheck);
            var kingPosition = PieceMoveHelpers.GetPositionOfKing(game, playerToCheck);
            return cellsInCheck[kingPosition.Row, kingPosition.Col];
        }

        public List<Move> GetPossibleMoves(GameState game, Cell position, Player player)
        {
            var piece = game.GetPiece(position);
            var possibleMoves = piece.GetPossibleMoves();
            possibleMoves = FilterForbiddenMoves(possibleMoves, game, player);
            return possibleMoves;
        }

        #region Helpers
        private bool PlayerCanPerformMove(GameState game, Player playerToCheck)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.Board[i, j].Player == playerToCheck)
                    {
                        var curPossibleMoves = GetPossibleMoves(game, new Cell(i, j), playerToCheck);
                        if (curPossibleMoves.Any())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private List<Move> FilterForbiddenMoves(List<Move> moves, GameState game, Player player)
        {
            var filteredMoves = new List<Move>();
            foreach (Move move in moves)
            {
                var kingIsInCheckAfterMove = KingIsInCheckAfterMove(move, game, player);

                if (!kingIsInCheckAfterMove)
                {
                    filteredMoves.Add(move);
                }
            }
            return filteredMoves;
        }

        private bool KingIsInCheckAfterMove(Move move, GameState game, Player player)
        {
            _executePieceMoveService.SimulateExecuteMove(game, move);
            var kingIsInCheck = KingIsInCheck(game, player);
            _executePieceMoveService.RollbackSimulatedMove(move);
            return kingIsInCheck;
        }
        #endregion   
    }
}
