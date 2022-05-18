using Chess.Models.Pieces;
using Chess.Moves;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models
{
    public class GameState : BaseModel
    {
        public MoveStack MoveStack { get; set; } = new MoveStack();
        public Piece[,] Board { get; private set; }
        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get
            {
                return _currentPlayer;
            }

            set
            {
                _currentPlayer = value;
                OnPropertyChanged();

            }
        }

        public GameState(bool initialize = true)
        {
            if (initialize)
            {
                InitializeBoard();
                CurrentPlayer = Player.White;
            }
        }

        public GameState Clone()
        {
            var newGame = new GameState(false);
            newGame.CurrentPlayer = CurrentPlayer;
            newGame.Board = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    newGame.Board[i, j] = Board[i, j].Clone();
                    newGame.Board[i, j].Game = newGame;
                }
            }

            newGame.MoveStack = MoveStack.Clone();
            foreach (var move in MoveStack.Moves)
            {
                move.Game = newGame;
                foreach (var action in move.Actions)
                {
                    action.Game = newGame;
                }
            }

            return newGame;
        }

        public Piece GetPiece(Cell position)
        {
            return Board[position.Row, position.Col];
        }

        public void SetBoardEntry(Cell position, Piece piece)
        {
            piece.Position = position;
            Board[position.Row, position.Col] = piece;
        }

        private void InitializeBoard()
        {
            Board = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SetBoardEntry(new Cell(i, j), new Empty(this));
                }
            }

            SetBoardEntry(new Cell(0, 0), new Rook(this, Player.Black));
            SetBoardEntry(new Cell(0, 1), new Knight(this, Player.Black));
            SetBoardEntry(new Cell(0, 2), new Bishop(this, Player.Black));
            SetBoardEntry(new Cell(0, 3), new Queen(this, Player.Black));
            SetBoardEntry(new Cell(0, 4), new King(this, Player.Black));
            SetBoardEntry(new Cell(0, 5), new Bishop(this, Player.Black));
            SetBoardEntry(new Cell(0, 6), new Knight(this, Player.Black));
            SetBoardEntry(new Cell(0, 7), new Rook(this, Player.Black));
            SetBoardEntry(new Cell(1, 0), new Pawn(this, Player.Black));
            SetBoardEntry(new Cell(1, 1), new Pawn(this, Player.Black));
            SetBoardEntry(new Cell(1, 2), new Pawn(this, Player.Black));
            SetBoardEntry(new Cell(1, 3), new Pawn(this, Player.Black));
            SetBoardEntry(new Cell(1, 4), new Pawn(this, Player.Black));
            SetBoardEntry(new Cell(1, 5), new Pawn(this, Player.Black));
            SetBoardEntry(new Cell(1, 6), new Pawn(this, Player.Black));
            SetBoardEntry(new Cell(1, 7), new Pawn(this, Player.Black));

            SetBoardEntry(new Cell(6, 0), new Pawn(this, Player.White));
            SetBoardEntry(new Cell(6, 1), new Pawn(this, Player.White));
            SetBoardEntry(new Cell(6, 2), new Pawn(this, Player.White));
            SetBoardEntry(new Cell(6, 3), new Pawn(this, Player.White));
            SetBoardEntry(new Cell(6, 4), new Pawn(this, Player.White));
            SetBoardEntry(new Cell(6, 5), new Pawn(this, Player.White));
            SetBoardEntry(new Cell(6, 6), new Pawn(this, Player.White));
            SetBoardEntry(new Cell(6, 7), new Pawn(this, Player.White));
            SetBoardEntry(new Cell(7, 0), new Rook(this, Player.White));
            SetBoardEntry(new Cell(7, 1), new Knight(this, Player.White));
            SetBoardEntry(new Cell(7, 2), new Bishop(this, Player.White));
            SetBoardEntry(new Cell(7, 3), new Queen(this, Player.White));
            SetBoardEntry(new Cell(7, 4), new King(this, Player.White));
            SetBoardEntry(new Cell(7, 5), new Bishop(this, Player.White));
            SetBoardEntry(new Cell(7, 6), new Knight(this, Player.White));
            SetBoardEntry(new Cell(7, 7), new Rook(this, Player.White));
        }
    }
}
