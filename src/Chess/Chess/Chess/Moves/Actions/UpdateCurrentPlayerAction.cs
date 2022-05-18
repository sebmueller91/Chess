using Chess.Models;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class UpdateCurrentPlayerAction : RevertableAction
    {
        public Player PlayerBeforeChange { get; set; }

        public UpdateCurrentPlayerAction(GameState game) : base(game)
        {
            PlayerBeforeChange = game.CurrentPlayer;
            Game = game;
        }

        public override RevertableAction Clone()
        {
            var newAction = new UpdateCurrentPlayerAction(Game);
            newAction.PlayerBeforeChange = PlayerBeforeChange;
            return newAction;
        }

        public override void Execute()
        {
            Game.CurrentPlayer = Helpers.GetOpposingPlayer(PlayerBeforeChange);
        }

        public override void Rollback()
        {
            Game.CurrentPlayer = PlayerBeforeChange;
        }
    }
}
