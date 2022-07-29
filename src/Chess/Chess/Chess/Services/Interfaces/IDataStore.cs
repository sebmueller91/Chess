using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chess.DataStore;
using Chess.Models;

namespace Chess.Services
{
    public interface IDataStoreService
    {
        DB_Game GetGameFromDatabase(string identifier);

        int InsertGameIntoDatabase(string identifier, GameState game, bool orientationReverted, Difficulty? difficulty = null, Player? aiPlayerColor = null);
    }
}
