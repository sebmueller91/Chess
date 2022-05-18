using Chess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class SetIsMovedAction : RevertableAction
    {
        private Cell m_position;

        public SetIsMovedAction(Cell position, GameState game) : base(game)
        {
            m_position = position;
        }

        public override void Execute()
        {
            if (m_position != null && Game?.Board != null)
            {
                Game.Board[m_position.Row,m_position.Col].IsMoved = true;
            }
        }

        public override void Rollback()
        {
            if (m_position != null && Game?.Board != null)
            {
                Game.Board[m_position.Row, m_position.Col].IsMoved = false;
            }
        }

        public override RevertableAction Clone()
        {
            return new SetIsMovedAction(m_position.Clone(), Game);
        }
    }
}
