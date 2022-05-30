using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chess.Config;
using Chess.DataStore;
using Chess.Models;
using Newtonsoft.Json;

namespace Chess.Services
{
    public class DataStoreService : IDataStoreService
    {
        private Database _database;

        private Database ChessDatabase
        {
            get
            {
                if (_database == null)
                {
                    _database = new Database();
                }
                return _database;
            }
        }

        public GameState GetGameFromDatabase(string identifier)
        {
            var games = ChessDatabase.GetGames();
            if (games == null
                || games.Where(x => x.Identifier.Equals(identifier)).FirstOrDefault() == null)
            {
                return null;
            }
            var gameObject = games.Where(x => x.Identifier.Equals(identifier)).FirstOrDefault().Game;
            if (String.IsNullOrWhiteSpace(gameObject))
            {
                return null;
            }
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                };
                return Newtonsoft.Json.JsonConvert.DeserializeObject<GameState>(gameObject, settings);
            }
            catch (NullReferenceException e)
            {
                // TODO: Add Logging
                return null;
            }
        }

        public int InsertGameIntoDatabase(string identifier, GameState game)
        {
            string gameObject = Newtonsoft.Json.JsonConvert.SerializeObject(game);

            var db_game = new DB_Game()
            {
                Identifier = identifier,
                Game = gameObject
            };
            return ChessDatabase.SaveGame(db_game);
        }
    }
}

