using Chess.Services;
using Chess.Views;
using System;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chess
{
    public partial class App : Application
    {
        public static UnityContainer UnityContainer { get; private set; }

        public App()
        {
            InitializeComponent();

            UnityContainer = new UnityContainer();
            RegisterServices();

            MainPage = new AppShell();
        }

        private void RegisterServices()
        {
            UnityContainer.RegisterType<IExecutePieceMoveService, ExecutePieceMoveService>();
            UnityContainer.RegisterType<IPieceMoveOptionsService, PieceMoveOptionsService>();
            UnityContainer.RegisterSingleton<IDataStoreService, DataStoreService>();
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
