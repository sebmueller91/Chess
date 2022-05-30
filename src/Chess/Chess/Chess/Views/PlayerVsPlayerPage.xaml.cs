using Chess.Config;
using Chess.Models;
using Chess.Models.Pieces;
using Chess.Services;
using Chess.Utils;
using Chess.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chess.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerVsPlayerPage : ContentPage, INotifyPropertyChanged
    {
        private PlayerVsPlayerViewModel _viewModel;
        private Button[,] _ChessGridButtons { get; set; }
        private Label[,] _ChessBackgroundLabels { get; set; }
        public PlayerVsPlayerPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.UnityContainer.Resolve<PlayerVsPlayerViewModel>();

            Initialize();

            _viewModel.ModelChanged += RenderChessGame;
            _viewModel.RequestRetrievePromotePawnType += async () => { await RequestTypeForPromotePawnFromUser(); };
            _viewModel.DisplayPlayerWonNotification += DisplayPlayerWonDialog;
            _viewModel.DisplayStalemateNotification += DisplayStalemateDialog;

            RenderChessGame();
        }

        private void Initialize()
        {
            _ChessGridButtons = new Button[8, 8];
            _ChessBackgroundLabels = new Label[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button button = new Button();
                    button.Margin = 0;
                    button.Padding = 0;

                    button.Background = GetCellColor(i, j);
                    button.BorderWidth = 0;

                    button.HeightRequest = 5;//TODO: Move into Constants
                    button.FontSize = 25;//TODO: Move into Constants
                    button.Text = _viewModel.Game.Board[i][j].Symbol;

                    var backgroundLabel = new Label()
                    {
                        TextColor = Constants.COLOR_BACKGROUND_LABEL_BACKGROUND,
                        Background = new SolidColorBrush(Helpers.GetBoardBackgroundColor(i, j)),
                        Text = Constants.TEXT_BACKGROUND_LABEL_SYMBOL,
                        FontAttributes = FontAttributes.Bold,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 30 //TODO: Move into Constants
                    };

                    _ChessGridButtons[i, j] = button;
                    _ChessBackgroundLabels[i, j] = backgroundLabel;
                    ChessGrid.Children.Add(backgroundLabel, j, i);
                    ChessGrid.Children.Add(button, j, i);
                }
            }
            AssignCellSelectedBindings();
        }

        protected override void OnAppearing()
        {
            _viewModel.LoadGameStateCommand?.Execute(null);
            AssignCellSelectedBindings();
        }

        //protected override void OnDisappearing()
        //{
        //    _viewModel.SaveCurrentGameStateCommand?.Execute(null);
        //}

        private async void ResetButtonClickedHandler(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Reset Game", "Do you really want to reset the game?", "Reset", "Cancel");
            if (answer)
            {
                _viewModel.ResetGameCommand?.Execute(null);
            }
        }

        private void RevertOrientationHandler(object sender, EventArgs e)
        {
            _viewModel.Game.OrientationReverted = !_viewModel.Game.OrientationReverted;
            AssignCellSelectedBindings(false);
            RenderChessGame();
            _viewModel.SaveCurrentGameStateCommand?.Execute(null);
        }

        private void DisplayPlayerWonDialog(Player player)
        {
            var message = $"{player.ToString()} won the game!";
            DisplayAlert("Checkmate", message, "Ok");
        }

        private void DisplayStalemateDialog(Player player)
        {
            var message = $"{player.ToString()} can not perform any valid moves!";
            DisplayAlert("Stalemate", message, "Ok");
        }

        private async Task RequestTypeForPromotePawnFromUser()
        {
            var queenText = "Queen";
            var knightText = "Knight";
            var rookText = "Rook";
            var bishtopText = "Bishop";
            var choice = await DisplayActionSheet("Promote Pawn to...", "Cancel", null, queenText, knightText, rookText, bishtopText);
            if (choice == null)
            {
                return;
            }
            else if (choice.Equals(queenText))
            {
                _viewModel.PromotePawnCommand.Execute(typeof(Queen));
            }
            else if (choice.Equals(knightText))
            {
                _viewModel.PromotePawnCommand.Execute(typeof(Knight));
            }
            else if (choice.Equals(rookText))
            {
                _viewModel.PromotePawnCommand.Execute(typeof(Rook));
            }
            else if (choice.Equals(bishtopText))
            {
                _viewModel.PromotePawnCommand.Execute(typeof(Bishop));
            }
        }

        private void RenderChessGame()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    _ChessGridButtons[getRowIndex(i), j].Text = GetCellText(i, j);
                    _ChessGridButtons[getRowIndex(i), j].Background = GetCellColor(i, j);
                    _ChessGridButtons[getRowIndex(i), j].TextColor = GetTextColor(i, j);
                }
            }
        }

        private int getRowIndex(int r)
        {
            return _viewModel.Game.OrientationReverted ? 7 - r : r;
        }

        private string GetCellText(int row, int col)
        {

            if (_viewModel.PossibleMovesForCurrentPiece != null
                       && _viewModel.PossibleMovesForCurrentPiece.Any(x => x.ToCell.Row == row && x.ToCell.Col == col)
                       && _viewModel.Game.Board[row][col].Player == Player.None)
            {
                return Constants.TEXT_POSSIBLE_MOVE;
            }
            else
            {
                return _viewModel.Game.Board[row][col].Symbol;
            }
        }

        private SolidColorBrush GetCellColor(int row, int col)
        {
            Color color;

            if (_viewModel.SelectedCell != null && _viewModel.SelectedCell.Row == row && _viewModel.SelectedCell.Col == col)
            {
                color = Constants.COLOR_SELECTED_CELL_BACKGROUND;
            }
            else if (_viewModel.PossibleMovesForCurrentPiece != null
                     && _viewModel.PossibleMovesForCurrentPiece.Any(x => x.ToCell.Row == row && x.ToCell.Col == col)
                     && _viewModel.Game.Board[row][col].Player == Helpers.GetOpposingPlayer(_viewModel.Game.CurrentPlayer))
            {
                color = Constants.COLOR_TRANSPARENT;
            }
            else if (_viewModel.KingsInCheck != null
                     && _viewModel.KingsInCheck.Any(x => x.Row == row && x.Col == col))
            {
                color = Constants.COLOR_TRANSPARENT;
            }
            else
            {
                color = Helpers.GetBoardBackgroundColor(row, col);
            }
            return new SolidColorBrush(color);
        }

        private Color GetTextColor(int row, int col)
        {
            if (_viewModel.PossibleMovesForCurrentPiece != null
                && _viewModel.PossibleMovesForCurrentPiece.Any(x => x.ToCell.Row == row && x.ToCell.Col == col)
                && _viewModel.Game.Board[row][col] is Empty)
            {
                return Constants.COLOR_POSSIBLE_MOVE_CELL;
            }

            if (_viewModel.Game.Board[row][col].Player == Player.White)
            {
                return Constants.COLOR_PLAYER_WHITE;
            }
            else
            {
                return Constants.COLOR_PLAYER_BLACK;
            }
        }

        private void AssignCellSelectedBindings(bool initializing = true)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (initializing)
                    {
                        _ChessGridButtons[i, j].RemoveBinding(Button.CommandProperty);
                    }
                    _ChessGridButtons[i, j].SetBinding(Button.CommandProperty, new Binding { Path = "CellSelectedCommand" });
                    var pos = new Tuple<int, int>(getRowIndex(i), j);
                    _ChessGridButtons[i, j].CommandParameter = pos;
                }
            }
        }
    }
}