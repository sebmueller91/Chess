using Chess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Services
{
    internal class ActiveGameProviderService
    {
        public GameState CurrentGame { get; private set; } = null;

        private static ActiveGameProviderService instance = null;
        private ActiveGameProviderService ()
        {
        }

        public static ActiveGameProviderService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActiveGameProviderService();
                }
                return instance;
            }
        }

        internal void RegisterCurrentGame(GameState game)
        {
            CurrentGame = game;
        }
    }
}
