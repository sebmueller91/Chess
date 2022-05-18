using Chess.Models.Pieces;
using Chess.Moves;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models
{
    public abstract class Piece : BaseModel
    {
        private Player m_Player;
        public Player Player
        {
            get
            {
                return m_Player;
            }
            private set
            {
                m_Player = value;
            }
        }

        private String m_Symbol;
        public String Symbol
        {
            get
            {
                return m_Symbol;
            }
            set
            {
                m_Symbol = value;
            }
        }

        public bool IsMoved { get; set; }

        private Cell m_Position = null;
        public Cell Position
        {
            get
            {
                m_Position = null;
                if (m_Position == null || Game.Board[m_Position.Row, m_Position.Col] != this)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (Game.Board[i, j] == this)
                            {
                                m_Position = new Cell(i, j);
                            }
                        }
                    }
                }
                return m_Position;
            }
            set { m_Position = value; }
        }
        public GameState Game { get; set; }
        public Piece(GameState game, Player player, String symbol) : this(game, player)
        {
            this.Symbol = symbol;
        }

        protected Piece(GameState game, Player player)
        {
            this.Game = game;
            this.Player = player;
        }

        public abstract Piece Clone();
        public Piece CloneProperties(Piece newPiece) { 
            newPiece.Symbol = this.Symbol;
            newPiece.IsMoved = this.IsMoved;
            newPiece.Position = this.Position.Clone();

            return newPiece;
        }

        public abstract List<Move> GetPossibleMoves();
    }
}
