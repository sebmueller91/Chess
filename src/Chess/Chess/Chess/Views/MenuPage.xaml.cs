using System;
using System.ComponentModel;
using Chess.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chess.Views
{
    public partial class MenuPage : ContentPage
    {
        private MenuViewModel _viewModel;

        public MenuPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MenuViewModel();
        }
    }
}