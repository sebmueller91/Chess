using Chess.Models;
using Chess.Models.Pieces;
using Chess.Moves.Actions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Chess.Utils
{
    public class BaseConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Piece) || objectType == typeof(RevertableAction));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            switch (jo["ObjType"].Value<string>())
            {
                case "King":
                    return JsonConvert.DeserializeObject<King>(jo.ToString(), SpecifiedSubclassConversion);
                case "Pawn":
                    return JsonConvert.DeserializeObject<Pawn>(jo.ToString(), SpecifiedSubclassConversion);
                case "Queen":
                    return JsonConvert.DeserializeObject<Queen>(jo.ToString(), SpecifiedSubclassConversion);
                case "Rook":
                    return JsonConvert.DeserializeObject<Rook>(jo.ToString(), SpecifiedSubclassConversion);
                case "Bishop":
                    return JsonConvert.DeserializeObject<Bishop>(jo.ToString(), SpecifiedSubclassConversion);
                case "Knight":
                    return JsonConvert.DeserializeObject<Knight>(jo.ToString(), SpecifiedSubclassConversion);
                case "Empty":
                    return JsonConvert.DeserializeObject<Empty>(jo.ToString(), SpecifiedSubclassConversion);
                case "CapturePieceAction":
                    return JsonConvert.DeserializeObject<CapturePieceAction>(jo.ToString(), SpecifiedSubclassConversion);
                case "MovePieceAction":
                    return JsonConvert.DeserializeObject<MovePieceAction>(jo.ToString(), SpecifiedSubclassConversion);
                case "PromotePawnAction":
                    return JsonConvert.DeserializeObject<PromotePawnAction>(jo.ToString(), SpecifiedSubclassConversion);
                case "SetIsMovedAction":
                    return JsonConvert.DeserializeObject<SetIsMovedAction>(jo.ToString(), SpecifiedSubclassConversion);
                case "UpdateCurrentPlayerAction":
                    return JsonConvert.DeserializeObject<UpdateCurrentPlayerAction>(jo.ToString(), SpecifiedSubclassConversion);
                default:
                    throw new Exception();
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}