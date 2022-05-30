using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Chess.Models
{
    public class Cell
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public override bool Equals(object cellObj)
        {
            if (!(cellObj is Cell)) return false;
            var cell = (Cell)cellObj;

            return this?.Row == cell?.Row && this?.Col == cell?.Col;
        }

        public static bool operator ==(Cell c1, Cell c2)
        {
            return c1?.Row == c2?.Row && c1?.Col == c2?.Col;
        }

        public static bool operator !=(Cell c1, Cell c2)
        {
            return c1?.Row != c2?.Row || c1?.Col != c2?.Col;
        }

        public Cell()
        {
        }

        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public Cell Clone()
        {
            return new Cell(Row, Col);
        }
    }
}
