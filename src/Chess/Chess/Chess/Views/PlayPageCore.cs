using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Chess.Config;
using Chess.Models;
using Chess.Models.Pieces;
using Chess.Utils;
using Chess.ViewModels;
using Xamarin.Forms;

namespace Chess.Views
{
    public class PlayPageCore : ContentPage, INotifyPropertyChanged
    {
        private BasePlayViewModel ViewModel { get; set; }
        private Grid Grid { get; set; }
        private Button[,] GridButtons { get; set; }
        private Label[,] ChessBackgroundLabels { get; set; }

        public PlayPageCore(BasePlayViewModel viewModel, Grid grid)
        {
            ViewModel = viewModel;
            Grid = grid;

            GridButtons = new Button[8, 8];
            ChessBackgroundLabels = new Label[8, 8];

            InitializeChessGrid();
            ViewModel.ModelChanged += RenderChessGame;
            RenderChessGame();
        }

        public void InitializeChessGrid()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Button button = new Button()
                    {
                        Margin = 0,
                        Padding = 0,
                        BorderWidth = 0,
                        Background = GetCellColor(i, j),
                        HeightRequest = 5,
                        FontSize = 25,
                        CornerRadius = 0,
                        Text = ViewModel.Game.Board[i][j].Symbol
                    };

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

                    GridButtons[i, j] = button;
                    ChessBackgroundLabels[i, j] = backgroundLabel;
                    Grid.Children.Add(backgroundLabel, j, i);
                    Grid.Children.Add(button, j, i);
                }
            }
            AssignCellSelectedBindings();
        }

        public void RevertOrientation()
        {
            ViewModel.OrientationReverted = !ViewModel.OrientationReverted;
            AssignCellSelectedBindings(false);
            ViewModel.SaveCurrentGameStateCommand.Execute(null);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var rowIndex = getRowIndex(i);
                    ChessBackgroundLabels[i,j].Background = new SolidColorBrush(Helpers.GetBoardBackgroundColor(rowIndex, j));
                }
            }

            RenderChessGame();
        }

        public void RenderChessGame()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    GridButtons[getRowIndex(i), j].Text = GetCellText(i, j);
                    GridButtons[getRowIndex(i), j].Background = GetCellColor(i, j);
                    GridButtons[getRowIndex(i), j].TextColor = GetTextColor(i, j);
                }
            }
        }

        public void AssignCellSelectedBindings(bool initializing = true)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (initializing)
                    {
                        GridButtons[i, j].RemoveBinding(Button.CommandProperty);
                    }
                    GridButtons[i, j].SetBinding(Button.CommandProperty, new Binding { Path = "CellSelectedCommand" });
                    var pos = new Tuple<int, int>(getRowIndex(i), j);
                    GridButtons[i, j].CommandParameter = pos;
                }
            }
        }

        private int getRowIndex(int r)
        {
            return ViewModel.OrientationReverted ? 7 - r : r;
        }

        private SolidColorBrush GetCellColor(int row, int col)
        {
            Color color;

            if (ViewModel.SelectedCell != null && ViewModel.SelectedCell.Row == row && ViewModel.SelectedCell.Col == col)
            {
                color = Constants.COLOR_SELECTED_CELL_BACKGROUND;
            }
            else if (ViewModel.PossibleMovesForCurrentPiece != null
                     && ViewModel.PossibleMovesForCurrentPiece.Any(x => x.ToCell.Row == row && x.ToCell.Col == col)
                     && ViewModel.Game.Board[row][col].Player == Helpers.GetOpposingPlayer(ViewModel.Game.CurrentPlayer))
            {
                color = Constants.COLOR_TRANSPARENT;
            }
            else if (ViewModel.KingsInCheck != null
                     && ViewModel.KingsInCheck.Any(x => x.Row == row && x.Col == col))
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
            if (ViewModel.PossibleMovesForCurrentPiece != null
                && ViewModel.PossibleMovesForCurrentPiece.Any(x => x.ToCell.Row == row && x.ToCell.Col == col)
                && ViewModel.Game.Board[row][col] is Empty)
            {
                return Constants.COLOR_POSSIBLE_MOVE_CELL;
            }

            if (ViewModel.Game.Board[row][col].Player == Player.White)
            {
                return Constants.COLOR_PLAYER_WHITE;
            }
            else
            {
                return Constants.COLOR_PLAYER_BLACK;
            }
        }

        private string GetCellText(int row, int col)
        {

            if (ViewModel.PossibleMovesForCurrentPiece != null
                       && ViewModel.PossibleMovesForCurrentPiece.Any(x => x.ToCell.Row == row && x.ToCell.Col == col)
                       && ViewModel.Game.Board[row][col].Player == Player.None)
            {
                return Constants.TEXT_POSSIBLE_MOVE;
            }
            else
            {
                return ViewModel.Game.Board[row][col].Symbol;
            }
        }
    }
}

