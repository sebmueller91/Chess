using Chess.Config;
using Chess.Models;
using Chess.Models.Pieces;
using Chess.Moves;
using Chess.Moves.Actions;
using Chess.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Cell = Chess.Models.Cell;

namespace Chess.Utils
{
    public static class Helpers
    {
        public static GameState GetCurrentGame()
        {
            return ActiveGameProviderService.Instance.CurrentGame;
        }

        public static GameState DeserializeGameState(string gameObject)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                };
                return Newtonsoft.Json.JsonConvert.DeserializeObject<GameState>(gameObject, settings);
            }
            catch (NullReferenceException e)
            {
                // TODO: Add Logging
                return null;
            }
        }

        public static T[][] TwoDimJaggedArray<T>(int rows, int cols)
        {
            var array = new T[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new T[cols];
            }
            return array;
        }
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

        public static List<Piece> GetAllPicesOfPlayer(GameState game, Player player)
        {
            var pieces = new List<Piece>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.Board[i][j].Player == player)
                    {
                        pieces.Add(game.Board[i][j]);
                    }
                }
            }
            return pieces;
        }

        public static Player GetRandomPlayer()
        {
            Random rand = new Random();

            if (rand.Next(0, 2) == 0)
            {
                return Player.Black;
            }
            else
            {
                return Player.White;
            }
        }
    }
}
