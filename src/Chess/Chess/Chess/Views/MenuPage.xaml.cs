using Chess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chess.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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