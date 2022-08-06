using Chess.Models;
using Chess.Models.Pieces;
using Chess.ViewModels;
using System;
using System.ComponentModel;
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
        private PlayPageCore Core { get; set; }

        public PlayerVsPlayerPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = App.UnityContainer.Resolve<PlayerVsPlayerViewModel>();

            Core = new PlayPageCore(_viewModel, ChessGrid, CurrentPlayerLabel);

            _viewModel.RequestRetrievePromotePawnType += async () => { await RequestTypeForPromotePawnFromUser(); };

            _viewModel.DisplayPlayerWonNotification += DisplayPlayerWonDialog;
            _viewModel.DisplayStalemateNotification += DisplayStalemateDialog;
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

        private void RevertOrientationHandler(object sender, EventArgs e)
        {
            Core.RevertOrientation();
        }
    }
}