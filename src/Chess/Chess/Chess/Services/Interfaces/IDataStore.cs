using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chess.Models;

namespace Chess.Services
{
    public interface IDataStoreService
    {
        GameState GetGameFromDatabase(string identifier);

        int InsertGameIntoDatabase(string identifier, GameState game);
    }
}
