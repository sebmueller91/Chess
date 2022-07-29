using Chess.Models;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.AI.Weights
{
    internal class EvaluationWeightsNormal : EvaluationWeights
    {
        public override int[,] GetSquareTableForPieceType(Type type)
        {
            if (type == typeof(King))
            {
                if (IsBeginningOfGame())
                {
                    return kingSquareWeightsBeginngOfGame;
                } else
                {
                    return kingSquareWeightsEndOfGame;
                }
            } else
            {
                return SquareTables[type];
            }
        }

        private int[,] kingSquareWeightsBeginngOfGame;
        private int[,] kingSquareWeightsEndOfGame;

        public EvaluationWeightsNormal(GameState game) : base(game)
        {
            PieceWeights[typeof(Pawn)] = 100;
            PieceWeights[typeof(Knight)] = 320;
            PieceWeights[typeof(Bishop)] = 330;
            PieceWeights[typeof(Rook)] = 500;
            PieceWeights[typeof(Queen)] = 900;
            PieceWeights[typeof(King)] = 20000;

            var pawnST = new int[8, 8] {{ 0, 0, 0, 0, 0, 0, 0, 0 },
                                         {50, 50, 50, 50, 50, 50, 50, 50},
                                             {10, 10, 20, 30, 30, 20, 10, 10},
                                             {5,  5, 10, 25, 25, 10,  5,  5},
                                             {0,  0,  0, 20, 20,  0,  0,  0},
                                             {5, -5,-10,  0,  0,-10, -5,  5},
                                             {5, 10, 10,-20,-20, 10, 10,  5},
                                             {0,  0,  0,  0,  0,  0,  0,  0}};
            SquareTables[typeof(Pawn)] = pawnST;

            var knightST = new int[8, 8] {{-50,-40,-30,-30,-30,-30,-40,-50 },
                                               {-40,-20,  0,  0,  0,  0,-20,-40},
                                               {-30,  0, 10, 15, 15, 10,  0,-30},
                                               {-30,  5, 15, 20, 20, 15,  5,-30},
                                               {-30,  0, 15, 20, 20, 15,  0,-30},
                                               {-30,  5, 10, 15, 15, 10,  5,-30},
                                               {-40,-20,  0,  5,  5,  0,-20,-40},
                                               {-50,-40,-30,-30,-30,-30,-40,-50}};
            SquareTables[typeof(Knight)] = knightST;

            var bishopST = new int[8, 8] {{-20,-10,-10,-10,-10,-10,-10,-20},
                                          {-10,  0,  0,  0,  0,  0,  0,-10},
                                          {-10,  0,  5, 10, 10,  5,  0,-10},
                                          {-10,  5,  5, 10, 10,  5,  5,-10},
                                          {-10,  0, 10, 10, 10, 10,  0,-10},
                                          {-10, 10, 10, 10, 10, 10, 10,-10},
                                          {-10,  5,  0,  0,  0,  0,  5,-10},
                                          {-20,-10,-10,-10,-10,-10,-10,-20}};
            SquareTables[typeof(Bishop)] = bishopST;

            var rookST = new int[8, 8] {{ 0,  0,  0,  0,  0,  0,  0,  0},
                                        {  5, 10, 10, 10, 10, 10, 10,  5},
                                        { -5,  0,  0,  0,  0,  0,  0, -5},
                                        { -5,  0,  0,  0,  0,  0,  0, -5},
                                        { -5,  0,  0,  0,  0,  0,  0, -5},
                                        { -5,  0,  0,  0,  0,  0,  0, -5},
                                        { -5,  0,  0,  0,  0,  0,  0, -5},
                                        {  0,  0,  0,  5,  5,  0,  0,  0}};
            SquareTables[typeof(Rook)] = rookST;

            var queenST = new int[8, 8] {{-20,-10,-10, -5, -5,-10,-10,-20},
                                        { -10,  0,  0,  0,  0,  0,  0,-10},
                                        { -10,  0,  5,  5,  5,  5,  0,-10},
                                        { -5,  0,  5,  5,  5,  5,  0, -5},
                                        {  0,  0,  5,  5,  5,  5,  0, -5},
                                        { -10,  5,  5,  5,  5,  5,  0,-10},
                                        { -10,  0,  5,  0,  0,  0,  0,-10},
                                        { -20,-10,-10, -5, -5,-10,-10,-20}};
            SquareTables[typeof(Queen)] = queenST;

            kingSquareWeightsBeginngOfGame
                        = new int[8, 8] {{-30,-40,-40,-50,-50,-40,-40,-30},
                                        { -30,-40,-40,-50,-50,-40,-40,-30},
                                        { -30,-40,-40,-50,-50,-40,-40,-30},
                                        { -30,-40,-40,-50,-50,-40,-40,-30},
                                        { -20,-30,-30,-40,-40,-30,-30,-20},
                                        { -10,-20,-20,-20,-20,-20,-20,-10},
                                        {  20, 20,  0,  0,  0,  0, 20, 20},
                                        {  20, 30, 10,  0,  0, 10, 30, 20}};

            kingSquareWeightsEndOfGame
                        = new int[8, 8] {{-50,-40,-30,-20,-20,-30,-40,-50},
                                        { -30,-20,-10,  0,  0,-10,-20,-30},
                                        { -30,-10, 20, 30, 30, 20,-10,-30},
                                        { -30,-10, 30, 40, 40, 30,-10,-30},
                                        { -30,-10, 30, 40, 40, 30,-10,-30},
                                        { -30,-10, 20, 30, 30, 20,-10,-30},
                                        { -30,-30,  0,  0,  0,  0,-30,-30},
                                        { -50,-30,-30,-30,-30,-30,-30,-50}};
        }

        private bool IsBeginningOfGame()
        {
            return PlayerIsInEndgame(Player.White) && PlayerIsInEndgame(Player.Black);
        }

        private bool PlayerIsInEndgame(Player player)
        {
            var numberMinorPieces = 0;
            var hasQueen = false;
            var hasRook = false;


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Game.Board[i][j].Player != player)
                    {
                        continue;
                    }

                    if (Game.Board[i][j] is Queen)
                    {
                        hasQueen = true;
                    } else if (Game.Board[i][j] is Rook)
                    {
                        hasRook = true;
                    } 
                    else if (Game.Board[i][j] is Knight 
                        || Game.Board[i][j] is Bishop)
                    {
                        numberMinorPieces++;
                    }
                }
            }

            return !hasQueen 
                || (!hasRook && numberMinorPieces < 2);
        }
    }
}
