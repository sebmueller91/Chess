using Chess.Config;
using Chess.Models;
using Chess.Models.Pieces;
using Chess.Services;
using Chess.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chess.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerVsAIPage : ContentPage, INotifyPropertyChanged
    {
        private PlayerVsAIViewModel _viewModel;
        private PlayPageCore Core { get; set; }

        public PlayerVsAIPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = App.UnityContainer.Resolve<PlayerVsAIViewModel>();

            Core = new PlayPageCore(_viewModel, ChessGrid, CurrentPlayerLabel);

            _viewModel.RequestRetrievePromotePawnType += async () => { await RequestTypeForPromotePawnFromUser(); };
            _viewModel.RequestDifficultyDialog += async () => { await RequestDifficultyFromUser(); };

            _viewModel.DisplayPlayerWonNotification += DisplayPlayerWonDialog;
            _viewModel.DisplayStalemateNotification += DisplayStalemateDialog;

            Core.ChessGameRendered += RenderChessGameAISpecifics;
        }

        public void RenderChessGameAISpecifics()
        {
            // Visualize move of AI
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((bool) _viewModel?.AIMoveVisualizationCells.Any(x => x.Row == i && x.Col == j))
                    {
                        Core.SetCellBackground(i,j,Constants.COLOR_AI_MOVE_BACKGROUND);
                    }
                }
            }
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

        protected override void OnAppearing()
        {
            _viewModel.LoadGameStateCommand?.Execute(null);
            Core.AssignCellSelectedBindings();
        }

        private async void ResetButtonClickedHandler(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Reset Game", "Do you really want to reset the game?", "Reset", "Cancel");
            if (answer)
            {
                _viewModel.ResetGameCommand?.Execute(null);
            }
        }

        public async Task RequestTypeForPromotePawnFromUser()
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

        public async Task RequestDifficultyFromUser()
        {
            var easyText = "Easy";
            var normalText = "Normal";
            var hardText = "Hard";
            var choice = await DisplayActionSheet("Choose Difficulty", null, null, easyText, normalText/*, hardText*/);
            if (choice == easyText)
            {
                _viewModel.DifficultySelectedCommand.Execute(Difficulty.Easy);
            }
            else if (choice == normalText)
            {
                _viewModel.DifficultySelectedCommand.Execute(Difficulty.Normal);
            } else
            {
                _viewModel.DifficultySelectedCommand.Execute(Difficulty.Hard);
            }
        }

        private void RevertOrientationHandler(object sender, EventArgs e)
        {
            Core.RevertOrientation();
        }
    }
}