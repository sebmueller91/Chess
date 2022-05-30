using Chess.Models;
using Chess.Models.Pieces;
using Chess.Moves;
using Chess.Services;
using Chess.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using Cell = Chess.Models.Cell;
using Chess.Config;

namespace Chess.ViewModels
{
    internal class PlayerVsPlayerViewModel : BaseViewModel
    {
        public event Action ModelChanged;
        public Command<Tuple<int, int>> CellSelectedCommand { get; private set; }
        public Command<Type> PromotePawnCommand { get; private set; }
        public Action RequestRetrievePromotePawnType { get; set; }
        public Command UndoLastMoveCommand { get; private set; }
        public Command RedoNextMoveCommand { get; private set; }

        public Command SaveCurrentGameStateCommand { get; private set; }
        public Command LoadGameStateCommand { get; private set; }


        public Command ResetGameCommand { get; private set; }

        public Action<Player> DisplayPlayerWonNotification { get; set; }
        public Action<Player> DisplayStalemateNotification { get; set; }

        public GameState Game { get; set; }
        public Cell SelectedCell { get; set; }
        public List<Moves.Move> PossibleMovesForCurrentPiece { get; private set; }

        public List<Cell> KingsInCheck { get; private set; } = new List<Cell>();

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

        private IExecutePieceMoveService _executePieceMoveService { get; set; }
        private IPieceMoveOptionsService _pieceMoveOptionsService { get; set; }
        private IDataStoreService _dataStore { get; set; }

        private Cell _pawnPromotionSelectedCell = null;

        Cell previouslySelectedCell = null;
        public PlayerVsPlayerViewModel(IExecutePieceMoveService executePieceMoveService,
            IPieceMoveOptionsService pieceMoveOptionsService,
            IDataStoreService dataStore)
        {
            Title = "Player VS Player";
            Game = new GameState();

            _executePieceMoveService = executePieceMoveService;
            _pieceMoveOptionsService = pieceMoveOptionsService;
            _dataStore = dataStore;

            CellSelectedCommand = new Command<Tuple<int, int>>(CellSelectedCommandHandler);
            UndoLastMoveCommand = new Command(UndoLastMoveCommandHandler);
            RedoNextMoveCommand = new Command(RedoNextMoveCommandHandler);
            PromotePawnCommand = new Command<Type>(PromotePawnCommandHandler);
            ResetGameCommand = new Command(ResetGameCommandHandler);
            SaveCurrentGameStateCommand = new Command(SaveCurrentGameStateCommandHandler);
            LoadGameStateCommand = new Command(LoadGameStateCommandHandler);

            ModelChanged += UpdateCanUndoLastMove;
            ModelChanged += UpdateCanRedoNextMove;
        }

        private void SaveCurrentGameStateCommandHandler()
        {
            _dataStore.InsertGameIntoDatabase(Constants.IDENTIFIER_GAMESTATE_PVP, Game);
        }

        private void LoadGameStateCommandHandler()
        {
            var loaded_game = _dataStore.GetGameFromDatabase(Constants.IDENTIFIER_GAMESTATE_PVP);
            Game = (loaded_game == null) ? new GameState() : loaded_game;

            ActiveGameProviderService.Instance.RegisterCurrentGame(Game);
            FireModelChangedEvent();
        }

        private void PromotePawnCommandHandler(Type type)
        {
            PossibleMovesForCurrentPiece = Helpers.FilterPromotePawnMovesExceptOfType(PossibleMovesForCurrentPiece, type);
            SelectedCell = _pawnPromotionSelectedCell;
            UpdateField();
        }


        private void CellSelectedCommandHandler(Tuple<int, int> position)
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

        private void UpdateField()
        {
            if (SelectedCell != null)
            {
                if (Game.Board[SelectedCell.Row][SelectedCell.Col].Player == Game.CurrentPlayer)
                {
                    PossibleMovesForCurrentPiece = _pieceMoveOptionsService.GetPossibleMoves(Game, SelectedCell, Game.CurrentPlayer);
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
                    }
                    PossibleMovesForCurrentPiece?.Clear();
                }
            }

            if (_pieceMoveOptionsService.IsCheckmate(Game, Game.CurrentPlayer))
            {
                DisplayPlayerWonNotification?.Invoke(Helpers.GetOpposingPlayer(Game.CurrentPlayer));
            }
            if (_pieceMoveOptionsService.IsStalemate(Game, Game.CurrentPlayer))
            {
                DisplayStalemateNotification?.Invoke(Helpers.GetOpposingPlayer(Game.CurrentPlayer));
            }

            SaveCurrentGameStateCommand.Execute(null);

            UpdateKingsInCheck();

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

        private void ResetGameCommandHandler(object obj)
        {
            Game = new GameState();
            ActiveGameProviderService.Instance.RegisterCurrentGame(Game);

            FireModelChangedEvent();
        }

        private void FireModelChangedEvent()
        {
            ModelChanged?.Invoke();
        }

        private void ResetSelection()
        {
            SelectedCell = previouslySelectedCell = null;
            PossibleMovesForCurrentPiece = new List<Move>();
            FireModelChangedEvent();
        }
    }
}
