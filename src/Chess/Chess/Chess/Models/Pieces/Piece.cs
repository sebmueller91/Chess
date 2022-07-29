using Chess.Models.Pieces;
using Chess.Moves;
using Chess.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Models
{
    [JsonConverter(typeof(BaseConverter))]
    public abstract class Piece : BaseModel
    {
        public string ObjType { get; set; }

        public Player Player { get; set; }
        
        public string Symbol { get; protected set; }

        public bool IsMoved { get; set; }

        public Cell Position { get; set; }

        public Piece(String symbol, string objType)
        {
            this.Symbol = symbol;
            this.ObjType = objType;
        }

        public Piece(Player player, String symbol, string objType) : this(symbol, objType)
        {
            this.Player = player;
        }

        public abstract Piece Clone();
        public Piece CloneProperties(Piece newPiece) { 
            newPiece.Symbol = this.Symbol;
            newPiece.IsMoved = this.IsMoved;
            newPiece.Position = this.Position.Clone();

            return newPiece;
        }

        public abstract List<Move> GetPossibleMoves(bool isActiveMove = true);
    }
}
