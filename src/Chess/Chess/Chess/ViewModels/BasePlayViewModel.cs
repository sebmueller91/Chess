using Chess.Config;
using Chess.Models;
using Chess.Moves;
using Chess.Services;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Cell = Chess.Models.Cell;

namespace Chess.ViewModels
{
    public abstract class BasePlayViewModel : BaseViewModel
    {
        #region Events
        public event Action ModelChanged;
        #endregion

        #region Commands
        public Command<Tuple<int, int>> CellSelectedCommand { get; protected set; }
        public Command<Type> PromotePawnCommand { get; protected set; }
        public Command SaveCurrentGameStateCommand { get; protected set; }
        public Command LoadGameStateCommand { get; protected set; }
        public Command ResetGameCommand { get; protected set; }
        #endregion

        #region Actions
        public Action RequestRetrievePromotePawnType { get; set; }
        public Action<Player> DisplayPlayerWonNotification { get; set; }
        public Action<Player> DisplayStalemateNotification { get; set; }
        #endregion

        #region Properties
        public GameState Game { get; set; }
        public Cell SelectedCell { get; set; }
        public List<Moves.Move> PossibleMovesForCurrentPiece { get; protected set; }
        public List<Cell> KingsInCheck { get; protected set; } = new List<Cell>();
        public bool OrientationReverted { get; set; } = false;

        protected IExecutePieceMoveService _executePieceMoveService { get; set; }
        protected IPieceMoveOptionsService _pieceMoveOptionsService { get; set; }
        protected IDataStoreService _dataStore { get; set; }

        protected Cell _pawnPromotionSelectedCell = null;

        protected Cell previouslySelectedCell = null;

        protected abstract string SAVE_IDENTIFIER { get; }

        protected bool nextMoveIsAIMove = false;
        #endregion

        #region Abstract Methods
        public abstract void SaveCurrentGameStateCommandHandler();

        public abstract void LoadGameStateCommandHandler();
        #endregion

        public BasePlayViewModel(IExecutePieceMoveService executePieceMoveService,
            IPieceMoveOptionsService pieceMoveOptionsService,
            IDataStoreService dataStore)
        {
            Game = new GameState();

            _executePieceMoveService = executePieceMoveService;
            _pieceMoveOptionsService = pieceMoveOptionsService;
            _dataStore = dataStore;

            CellSelectedCommand = new Command<Tuple<int, int>>(CellSelectedCommandHandler);

            PromotePawnCommand = new Command<Type>(PromotePawnCommandHandler);
            ResetGameCommand = new Command(ResetGameCommandHandler);
            SaveCurrentGameStateCommand = new Command(SaveCurrentGameStateCommandHandler);
            LoadGameStateCommand = new Command(LoadGameStateCommandHandler);
        }

        protected virtual void ResetGameCommandHandler(object obj)
        {
            Game = new GameState();
            ActiveGameProviderService.Instance.RegisterCurrentGame(Game);

            SaveCurrentGameStateCommand.Execute(null);
            FireModelChangedEvent();
        }

        protected void ResetSelection()
        {
            SelectedCell = previouslySelectedCell = null;
            PossibleMovesForCurrentPiece = new List<Move>();
            FireModelChangedEvent();
        }

        protected void UpdateField()
        {
            var gameStateChanged = false;
            if (SelectedCell != null)
            {
                if (Game.Board[SelectedCell.Row][SelectedCell.Col].Player == Game.CurrentPlayer)
                {
                    PossibleMovesForCurrentPiece = _pieceMoveOptionsService.GetPossibleMoves(Game, SelectedCell);
                    gameStateChanged = true;
                }
                else if (previouslySelectedCell != null
                         && Game.Board[previouslySelectedCell.Row][previouslySelectedCell.Col].Player == Game.CurrentPlayer
                         && Game.Board[SelectedCell.Row][SelectedCell.Col].Player != Game.CurrentPlayer)
                {
                    if (PossibleMovesForCurrentPiece.Any(x => x.ToCell == SelectedCell))
                    {
                        var movetoExecute = PossibleMovesForCurrentPiece.First(x => x.ToCell == SelectedCell);
                        _executePieceMoveService.ExecuteMove(Game, movetoExecute);
                        SelectedCell = previouslySelectedCell = null;
                        gameStateChanged = true;
                        nextMoveIsAIMove = true;
                    }
                    PossibleMovesForCurrentPiece?.Clear();
                }
            }

            if (_pieceMoveOptionsService.IsCheckmate(Game, Game.CurrentPlayer))
            {
                DisplayPlayerWonNotification?.Invoke(Helpers.GetOpposingPlayer(Game.CurrentPlayer));
                gameStateChanged = true;
            }
            if (_pieceMoveOptionsService.IsStalemate(Game, Game.CurrentPlayer))
            {
                DisplayStalemateNotification?.Invoke(Helpers.GetOpposingPlayer(Game.CurrentPlayer));
                gameStateChanged = true;
            }

            if (gameStateChanged)
            {
                SaveCurrentGameStateCommand.Execute(null);
            }

            UpdateKingsInCheck();

            FireModelChangedEvent();
        }

        private void PromotePawnCommandHandler(Type type)
        {
            PossibleMovesForCurrentPiece = Helpers.FilterPromotePawnMovesExceptOfType(PossibleMovesForCurrentPiece, type);
            SelectedCell = _pawnPromotionSelectedCell;
            UpdateField();
        }


        protected virtual void CellSelectedCommandHandler(Tuple<int, int> position)
        {
            previouslySelectedCell = SelectedCell;
            var newlySelectedCell = new Cell(position.Item1, position.Item2);

            if (Helpers.MoveToPositionPromotesPawn(PossibleMovesForCurrentPiece, newlySelectedCell))
            {
                _pawnPromotionSelectedCell = newlySelectedCell;
                RequestRetrievePromotePawnType?.Invoke();
            }
            else
            {
                SelectedCell = new Cell(position.Item1, position.Item2);
                UpdateField();
            }
            FireModelChangedEvent();
        }

        private void UpdateKingsInCheck()
        {
            KingsInCheck?.Clear();
            if (_pieceMoveOptionsService.KingIsInCheck(Game, Player.White))
            {
                KingsInCheck.Add(PieceMoveHelpers.GetPositionOfKing(Game, Player.White));
            }
            if (_pieceMoveOptionsService.KingIsInCheck(Game, Player.Black))
            {
                KingsInCheck.Add(PieceMoveHelpers.GetPositionOfKing(Game, Player.Black));
            }
            FireModelChangedEvent();
        }

        protected void FireModelChangedEvent()
        {
            ModelChanged?.Invoke();
        }
    }
}


