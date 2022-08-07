using System;
using System.ComponentModel;
using Chess.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chess.Views
{
    public partial class AboutPage : ContentPage
    {
        private AboutViewModel _viewModel;

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AboutViewModel();
        }
    }
}