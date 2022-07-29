using Chess.Models;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.AI
{
    public class EvaluationWeights
    {
        public Dictionary<Type, int> PieceWeights { get; }
        public virtual int[,] GetSquareTableForPieceType(Type type)
        {
            return SquareTables[type];
        }

        protected virtual Dictionary<Type, int[,]> SquareTables { get; set; }

        public GameState Game { get; private set; }

        public EvaluationWeights(GameState game)
        {
            Game = game;

            PieceWeights = new Dictionary<Type, int>();
            PieceWeights.Add(typeof(Pawn), 10);
            PieceWeights.Add(typeof(Knight), 31);
            PieceWeights.Add(typeof(Bishop), 32);
            PieceWeights.Add(typeof(Rook), 50);
            PieceWeights.Add(typeof(Queen), 90);
            PieceWeights.Add(typeof(King), 1000);

            SquareTables = new Dictionary<Type, int[,]>();
            var pawnSquareTable = new int[8, 8];
            SquareTables.Add(typeof(Pawn), pawnSquareTable);
            var knightSquareTable = new int[8, 8];
            SquareTables.Add(typeof(Knight), knightSquareTable);
            var bishopSquareTable = new int[8, 8];
            SquareTables.Add(typeof(Bishop), bishopSquareTable);
            var rookSquareTable = new int[8, 8];
            SquareTables.Add(typeof(Rook), rookSquareTable);
            var queenSquareTable = new int[8, 8];
            SquareTables.Add(typeof(Queen), queenSquareTable);
            var kingSquareTable = new int[8, 8];
            SquareTables.Add(typeof(King), kingSquareTable);
        }
    }
}
