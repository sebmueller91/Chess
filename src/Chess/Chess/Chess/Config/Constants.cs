using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Chess.Config
{
    public static class Constants
    {
        public static readonly string IDENTIFIER_GAMESTATE_PVP = "IDENTIFIER_GAMESTATE_PVP";
        public static readonly string IDENTIFIER_GAMESTATE_PVA = "IDENTIFIER_GAMESTATE_PVA";

        public const string DatabaseFilename = "chess.db3";
        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }

        public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;


        public static readonly Color COLOR_TRANSPARENT = Xamarin.Forms.Color.FromHex("00FBC1BC");
        public static readonly Color COLOR_SELECTED_CELL_BACKGROUND = Xamarin.Forms.Color.FromHex("66ff66");
        public static readonly Color COLOR_PLAYER_WHITE = Color.White;
        public static readonly Color COLOR_PLAYER_BLACK = Color.Black;
        public static readonly Color COLOR_POSSIBLE_MOVE_CELL = Color.DarkCyan;
        public static readonly Color COLOR_BOARD_BACKGROUND_BLACK = Color.DarkGray;
        public static readonly Color COLOR_BOARD_BACKGROUND_WHITE = Color.FromHex("336600");
        public static readonly Color COLOR_BACKGROUND_LABEL_BACKGROUND = Color.Red;
        public static readonly Color COLOR_AI_MOVE_BACKGROUND = Color.DarkOliveGreen;

        public static readonly string TEXT_POSSIBLE_MOVE = "\u2B24";
        public static readonly string TEXT_BISHOP = "\u265D";
        public static readonly string TEXT_EMPTY = "";
        public static readonly string TEXT_KING = "\u265A";
        public static readonly string TEXT_KNIGHT = "\u265E";
        public static readonly string TEXT_PAWN = "\u265F";
        public static readonly string TEXT_QUEEN = "\u265B";
        public static readonly string TEXT_ROOK = "\u265C";
        public static readonly string TEXT_BACKGROUND_LABEL_SYMBOL = "\u2B24";

    }
}
