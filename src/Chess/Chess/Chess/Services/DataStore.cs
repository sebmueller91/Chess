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

        public DB_Game GetGameFromDatabase(string identifier)
        {
            var games = ChessDatabase.GetGames();
            if (games == null
                || games.Where(x => x.Identifier.Equals(identifier)).FirstOrDefault() == null)
            {
                return null;
            }
            return games.Where(x => x.Identifier.Equals(identifier)).FirstOrDefault();
        }

        public int InsertGameIntoDatabase(string identifier, GameState game, bool orientationReverted, Difficulty? difficulty, Player? aiPlayerColor)
        {
            string gameObject = Newtonsoft.Json.JsonConvert.SerializeObject(game);

            var db_game = new DB_Game()
            {
                Identifier = identifier,
                Game = gameObject,
                OrientationReverted = orientationReverted,
                Difficulty = difficulty ?? Difficulty.Easy,
                AIPlayerColor = aiPlayerColor ?? Player.None
            };
            return ChessDatabase.SaveGame(db_game);
        } 
    }
}

