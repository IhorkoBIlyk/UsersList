using System;
using System.Net;
using UsersList.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UsersList
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            DependencyService.Register<IUserService, UserService>();

            ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;



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
