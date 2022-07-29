using Chess.ViewModels;
using Chess.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Chess
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MenuPage), typeof(MenuPage));
            Routing.RegisterRoute(nameof(PlayerVsPlayerPage), typeof(PlayerVsPlayerPage));
            Routing.RegisterRoute(nameof(PlayerVsAIPage), typeof(PlayerVsAIPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
