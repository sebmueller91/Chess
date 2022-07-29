using Chess.Models;
using Chess.Services;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    internal class UpdateCurrentPlayerAction : RevertableAction
    {
        public Player PlayerBeforeChange { get; set; }

        public UpdateCurrentPlayerAction() : base("UpdateCurrentPlayerAction")
        { 
        }

        public override RevertableAction Clone()
        {
            var newAction = new UpdateCurrentPlayerAction();
            newAction.PlayerBeforeChange = PlayerBeforeChange;
            return newAction;
        }

        public override void Execute()
        {
            PlayerBeforeChange = Helpers.GetCurrentGame().CurrentPlayer;
            Helpers.GetCurrentGame().CurrentPlayer = Helpers.GetOpposingPlayer(PlayerBeforeChange);
        }

        public override void Rollback()
        {
            Helpers.GetCurrentGame().CurrentPlayer = PlayerBeforeChange;
        }
    }
}
