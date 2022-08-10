using Chess.AI;
using Chess.Config;
using Chess.Models;
using Chess.Moves;
using Chess.Services;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Timers;
using Xamarin.Forms;
using Cell = Chess.Models.Cell;

namespace Chess.ViewModels
{
    public class PlayerVsAIViewModel : BasePlayViewModel
    {
        #region Properties, Actions, Commands
        public Action RequestDifficultyDialog { get; set; }
        public Command<Difficulty> DifficultySelectedCommand { get; protected set; }

        protected override string SAVE_IDENTIFIER { get => Constants.IDENTIFIER_GAMESTATE_PVA; }

        private AIMoveCalculationService aiMoveCalculationService { get; set; }

        #region Difficulty
        private Difficulty m_difficulty;
        public Difficulty Difficulty
        {
            get
            {
                return m_difficulty;
            }
            set
            {
                if (m_difficulty != value)
                {
                    m_difficulty = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region PlayerColor
        private Player m_PlayerColor;
        public Player PlayerColor
        {
            get
            {
                return m_PlayerColor;
            }
            set
            {
                if (m_PlayerColor != value)
                {
                    m_PlayerColor = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region PlayerColor
        public Player AIPlayerColor // TODO: Remove nullables, wont crash anyway
        {
            get
            {
                return Helpers.GetOpposingPlayer(PlayerColor);
            } set
            {
                PlayerColor = Helpers.GetOpposingPlayer(value);
                OnPropertyChanged();
            }
        }
        #endregion

        private Move _aiMove;
        private Timer _aiMoveTimer;
        bool _aiMoveBeingExecuted = false;
        #endregion

        public PlayerVsAIViewModel(IExecutePieceMoveService executePieceMoveService,
            IPieceMoveOptionsService pieceMoveOptionsService,
            IDataStoreService dataStore) : base(executePieceMoveService, pieceMoveOptionsService, dataStore)
        {
            Title = "Player VS AI";
            Game = new GameState();

            DifficultySelectedCommand = new Command<Difficulty>(DifficultySelectedCommandHandler);
            ModelChanged += OnModelChanged;
        }

        protected override void CellSelectedCommandHandler(Tuple<int, int> position)
        {
            if (Game.CurrentPlayer == PlayerColor)
            {
                base.CellSelectedCommandHandler(position);
            } else // AI is currently performing move -> mark selected cell but do nothing else
            {
                SelectedCell = new Cell(position.Item1, position.Item2);
            }
        }

        private void DifficultySelectedCommandHandler(Difficulty difficulty)
        {
            Difficulty = difficulty;
            CreateNewAIGame(difficulty);
            nextMoveIsAIMove = (Game.CurrentPlayer == AIPlayerColor); // AI must perform first move

            ActiveGameProviderService.Instance.RegisterCurrentGame(Game);

            SaveCurrentGameStateCommand.Execute(null);
            ResetSelection();
            FireModelChangedEvent();
        }

        public override void SaveCurrentGameStateCommandHandler()
        {
            _dataStore.InsertGameIntoDatabase(SAVE_IDENTIFIER, Game, OrientationReverted, Difficulty, AIPlayerColor);
        }

        public override void LoadGameStateCommandHandler()
        {
            var loaded_game = _dataStore.GetGameFromDatabase(SAVE_IDENTIFIER);
            if (loaded_game == null || loaded_game.Game == null)
            {
                RequestDifficultyDialog?.Invoke();
            }
            else
            {
                Game = Helpers.DeserializeGameState(loaded_game.Game);
                ActiveGameProviderService.Instance.RegisterCurrentGame(Game);
                OrientationReverted = loaded_game.OrientationReverted;
                Difficulty = loaded_game.Difficulty;
                AIPlayerColor = loaded_game.AIPlayerColor;
                aiMoveCalculationService = new AIMoveCalculationService((Difficulty) Difficulty, Game, (Player) AIPlayerColor);
                nextMoveIsAIMove = (Game.CurrentPlayer == AIPlayerColor);
            }
            
            FireModelChangedEvent();
        }

        protected override void ResetGameCommandHandler(object obj)
        {
            RequestDifficultyDialog?.Invoke();
        }

        private void CreateNewAIGame(Difficulty difficulty)
        {
            Game = new GameState();
            AIPlayerColor = Helpers.GetRandomPlayer();
            aiMoveCalculationService = new AIMoveCalculationService(difficulty, Game, (Player) AIPlayerColor);
        }

        private void OnModelChanged()
        {
            if (nextMoveIsAIMove)
            {
                DoAIMove();
            }
        }

        private void DoAIMove()
        {
            if (_aiMoveBeingExecuted)
            {
                return;
            }

            _aiMove = aiMoveCalculationService.GetNextMoveForPlayer();
            SelectedCell = _aiMove.FromCell;
            PossibleMovesForCurrentPiece = new List<Move>();
            PossibleMovesForCurrentPiece.Add(_aiMove);

            // Delay move execution so that it will be visualized
            _aiMoveBeingExecuted = true;
            _aiMoveTimer = new Timer(Constants.AIMoveDelay);
            _aiMoveTimer.Elapsed += (sender, e) => HandleTimer();
            _aiMoveTimer.Start();

            FireModelChangedEvent();
        }

        private void HandleTimer()
        {
            if (_aiMove == null)
            {
                return;
            }
            _aiMoveBeingExecuted = false;
            _aiMoveTimer.Dispose();
            SelectedCell = null;
            PossibleMovesForCurrentPiece = new List<Move>();           
            _executePieceMoveService.ExecuteMove(Game, _aiMove);
            _aiMove = null;
            nextMoveIsAIMove = false;
            UpdateField();
            SaveCurrentGameStateCommand.Execute(null);
        }
    }
}


