using Chess.Models;
using Chess.Moves;
using Chess.Services;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Cell = Chess.Models.Cell;
using Chess.Config;

namespace Chess.ViewModels
{
    public class PlayerVsPlayerViewModel : BasePlayViewModel
    {
        protected override string SAVE_IDENTIFIER { get => Constants.IDENTIFIER_GAMESTATE_PVP; }

        #region Commands
        public Command UndoLastMoveCommand { get; private set; }
        public Command RedoNextMoveCommand { get; private set; }
        #endregion

        #region IsUndoLastMoveEnabled
        private bool _isUndoLastMoveEnabled = false;
        public bool IsUndoLastMoveEnabled
        {
            get
            {
                return _isUndoLastMoveEnabled;
            }
            set
            {
                _isUndoLastMoveEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region _isRedoNextMoveEnabled
        private bool _isRedoNextMoveEnabled = false;
        public bool IsRedoNextMoveEnabled
        {
            get
            {
                return _isRedoNextMoveEnabled;
            }
            set
            {
                _isRedoNextMoveEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public PlayerVsPlayerViewModel(IExecutePieceMoveService executePieceMoveService,
            IPieceMoveOptionsService pieceMoveOptionsService,
            IDataStoreService dataStore) : base(executePieceMoveService, pieceMoveOptionsService, dataStore)
        {
            Title = "Player VS Player";

            UndoLastMoveCommand = new Command(UndoLastMoveCommandHandler);
            RedoNextMoveCommand = new Command(RedoNextMoveCommandHandler);

            ModelChanged += UpdateCanUndoLastMove;
            ModelChanged += UpdateCanRedoNextMove;
        }

        public override void SaveCurrentGameStateCommandHandler()
        {
            _dataStore.InsertGameIntoDatabase(SAVE_IDENTIFIER, Game, OrientationReverted);
        }

        public override void LoadGameStateCommandHandler()
        {
            var loaded_game = _dataStore.GetGameFromDatabase(SAVE_IDENTIFIER);
            if (loaded_game == null || loaded_game.Game == null)
            {
                Game = new GameState();
            }
            else
            {
                Game = Helpers.DeserializeGameState(loaded_game.Game);
                OrientationReverted = loaded_game.OrientationReverted;
            }

            ActiveGameProviderService.Instance.RegisterCurrentGame(Game);
            FireModelChangedEvent();
        }

        private void UndoLastMoveCommandHandler(object obj)
        {
            Game.MoveStack.RollbackLastMove();
            ResetSelection();
            UpdateField();
            FireModelChangedEvent();
        }

        public void UpdateCanUndoLastMove()
        {
            IsUndoLastMoveEnabled = Game.MoveStack.DoneActionsOnStack();
        }

        private void RedoNextMoveCommandHandler(object obj)
        {
            Game.MoveStack.RedoNextMoveOnStack();
            ResetSelection();
            UpdateField();
            FireModelChangedEvent();
        }

        private void UpdateCanRedoNextMove()
        {
            IsRedoNextMoveEnabled = Game.MoveStack.UndoneActionsOnStack();
        }
    }
}
