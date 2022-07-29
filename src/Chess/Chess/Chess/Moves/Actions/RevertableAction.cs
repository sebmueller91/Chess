using Chess.Models;
using Chess.Services;
using Chess.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Moves.Actions
{
    [JsonConverter(typeof(BaseConverter))]
    public abstract class RevertableAction
    {
        public string ObjType { get; protected set; }

        public RevertableAction(string objType)
        {
            ObjType = objType;
        }
        public abstract RevertableAction Clone();

        public abstract void Execute();

        public abstract void Rollback();
    }
}
