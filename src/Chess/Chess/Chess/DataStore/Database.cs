using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chess.Config;
using SQLite;

namespace Chess.DataStore
{

    public class Database
    {
        private readonly SQLiteConnection _database;

        public Database()
        {
            _database = new SQLiteConnection (Constants.DatabasePath, Constants.Flags);

            _database.CreateTable<DB_Game>();
        }

        public List<DB_Game> GetGames()
        {
            try { 
                return _database.Table<DB_Game>().ToList();
            } catch (Exception e)
            {
                return null;
            }
        }

        public int SaveGame(DB_Game game)
        {
             return _database.InsertOrReplace(game);
        }
    }
}

