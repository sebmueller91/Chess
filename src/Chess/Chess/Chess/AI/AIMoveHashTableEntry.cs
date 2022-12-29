using System;
using Chess.Models;

namespace Chess.AI
{
    public class AIMoveHashTableEntry
    {
        public Player CurrentPlayer { get; set; }
        public Piece[][] Board { get; set; }

        public AIMoveHashTableEntry(Player player, Piece[][] board)
        {
            CurrentPlayer = player;
            Board = board;
        }

        public AIMoveHashTableEntry(Piece[][] board)
        {
            Board = board;
        }
    }
}

