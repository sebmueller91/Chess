using Chess.Models.Pieces;
using Chess.Moves;
using Chess.Services;
using Chess.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models
{
    public class GameState
    {
        public MoveStack MoveStack { get; set; }
        public Piece[][] Board { get; set; }
        public Player _currentPlayer { get; set; }
        public Player CurrentPlayer
        {
            get
            {
                return _currentPlayer;
            }

            set
            {
                _currentPlayer = value;

            }
        }

        public bool OrientationReverted { get; set; } = false;

        [JsonConstructor]
        private GameState()
        {
        }

        public GameState(bool initialize = true)
        {
            if (initialize)
            {
                InitializeBoard();
                CurrentPlayer = Player.White;
                MoveStack = new MoveStack();
            }
        }

        public GameState Clone()
        {
            var newGame = new GameState(false);
            newGame.CurrentPlayer = CurrentPlayer;
            newGame.Board = Helpers.TwoDimJaggedArray<Piece>(8, 8);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    newGame.Board[i][j] = Board[i][j].Clone();
                }
            }

            newGame.MoveStack = MoveStack.Clone();

            return newGame;
        }

        public Piece GetPiece(Cell position)
        {
            return Board[position.Row][position.Col];
        }

        public void SetBoardEntry(Cell position, Piece piece)
        {
            piece.Position = position;
            Board[position.Row][position.Col] = piece;
        }

        private void InitializeBoard()
        {
            Board = Helpers.TwoDimJaggedArray<Piece>(8, 8);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SetBoardEntry(new Cell(i, j), new Empty());
                }
            }

            SetBoardEntry(new Cell(0, 0), new Rook(Player.Black));
            SetBoardEntry(new Cell(0, 1), new Knight(Player.Black));
            SetBoardEntry(new Cell(0, 2), new Bishop(Player.Black));
            SetBoardEntry(new Cell(0, 3), new Queen(Player.Black));
            SetBoardEntry(new Cell(0, 4), new King(Player.Black));
            SetBoardEntry(new Cell(0, 5), new Bishop(Player.Black));
            SetBoardEntry(new Cell(0, 6), new Knight(Player.Black));
            SetBoardEntry(new Cell(0, 7), new Rook(Player.Black));
            SetBoardEntry(new Cell(1, 0), new Pawn(Player.Black));
            SetBoardEntry(new Cell(1, 1), new Pawn(Player.Black));
            SetBoardEntry(new Cell(1, 2), new Pawn(Player.Black));
            SetBoardEntry(new Cell(1, 3), new Pawn(Player.Black));
            SetBoardEntry(new Cell(1, 4), new Pawn(Player.Black));
            SetBoardEntry(new Cell(1, 5), new Pawn(Player.Black));
            SetBoardEntry(new Cell(1, 6), new Pawn(Player.Black));
            SetBoardEntry(new Cell(1, 7), new Pawn(Player.Black));

            SetBoardEntry(new Cell(6, 0), new Pawn(Player.White));
            SetBoardEntry(new Cell(6, 1), new Pawn(Player.White));
            SetBoardEntry(new Cell(6, 2), new Pawn(Player.White));
            SetBoardEntry(new Cell(6, 3), new Pawn(Player.White));
            SetBoardEntry(new Cell(6, 4), new Pawn(Player.White));
            SetBoardEntry(new Cell(6, 5), new Pawn(Player.White));
            SetBoardEntry(new Cell(6, 6), new Pawn(Player.White));
            SetBoardEntry(new Cell(6, 7), new Pawn(Player.White));
            SetBoardEntry(new Cell(7, 0), new Rook(Player.White));
            SetBoardEntry(new Cell(7, 1), new Knight(Player.White));
            SetBoardEntry(new Cell(7, 2), new Bishop(Player.White));
            SetBoardEntry(new Cell(7, 3), new Queen(Player.White));
            SetBoardEntry(new Cell(7, 4), new King(Player.White));
            SetBoardEntry(new Cell(7, 5), new Bishop(Player.White));
            SetBoardEntry(new Cell(7, 6), new Knight(Player.White));
            SetBoardEntry(new Cell(7, 7), new Rook(Player.White));
        }
    }
}
