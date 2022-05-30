using System;
using Chess.Models;
using SQLite;

namespace Chess.DataStore
{
	public class DB_Game
	{
        [PrimaryKey]
		public string Identifier { get; set; }
		public string Game { get; set; }
	}
}

