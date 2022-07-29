using Chess.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Chess.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand PlayerVsPlayerButtonClickedCommand { private set; get; }
        public ICommand PlayerVsAIButtonClickedCommand { private set; get; }

        public MenuViewModel()
        {
            Title = "Menu";

            PlayerVsPlayerButtonClickedCommand = new Command(PlayerVsPlayerButtonClickedCommandHandler);
            PlayerVsAIButtonClickedCommand = new Command(PlayerVsAIButtonClickedCommandHandler);
        }        

        private async void PlayerVsPlayerButtonClickedCommandHandler()
        {
            await Shell.Current.GoToAsync(nameof(PlayerVsPlayerPage));
        }

        private async void PlayerVsAIButtonClickedCommandHandler()
        {
            await Shell.Current.GoToAsync(nameof(PlayerVsAIPage));
        }
    }
}
