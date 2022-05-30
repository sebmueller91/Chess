using Chess.Models;
using Chess.Moves.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Utils
{
    public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if ((typeof(Piece).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                || (typeof(RevertableAction).IsAssignableFrom(objectType) && !objectType.IsAbstract))
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }
}
