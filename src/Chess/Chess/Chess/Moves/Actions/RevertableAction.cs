using Chess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    public abstract class RevertableAction
    {
        public GameState Game { get; set; }

        public RevertableAction(GameState game)
        {
            this.Game = game;
        }
        public abstract RevertableAction Clone();

        public abstract void Execute();

        public abstract void Rollback();
    }
}
