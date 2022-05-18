using Chess.Config;
using Chess.Models;
using Chess.Models.Pieces;
using Chess.Moves;
using Chess.Moves.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Cell = Chess.Models.Cell;

namespace Chess.Utils
{
    public static class Helpers
    {
        public static Player GetOpposingPlayer(Player player)
        {
            if (player == Player.Black)
            {
                return Player.White;
            }
            else if (player == Player.White)
            {
                return Player.Black;
            }
            return Player.None;
        }

        public static Color GetBoardBackgroundColor(int row, int col)
        {
            if ((row % 2 == 0 && col % 2 == 0) || (row % 2 != 0 && col % 2 != 0))
            {
                return Constants.COLOR_BOARD_BACKGROUND_BLACK;
            }
            else
            {
                return Constants.COLOR_BOARD_BACKGROUND_WHITE;
            }
        }

        public static bool MoveToPositionPromotesPawn(List<Move> possibleMoves, Cell position)
        {
            if (possibleMoves == null) return false;
            return possibleMoves.Any(x => x.ToCell == position && x.Actions.Any(y => y is PromotePawnAction));
        }

        public static List<Move> FilterPromotePawnMovesExceptOfType(List<Move> possibleMoves, Type type)
        {
            var filteredMoves = new List<Move>();
            foreach (var move in possibleMoves)
            {
                if (move.Actions.Any(x => x is PromotePawnAction && (x as PromotePawnAction).PieceType != type))
                {
                    continue;
                }
                filteredMoves.Add(move);
            }
            return filteredMoves;
        }
    }
}
