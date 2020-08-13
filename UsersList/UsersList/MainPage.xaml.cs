using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersList.Common.Models.Entities;
using UsersList.Forms;
using UsersList.Forms.Views;
using UsersList.Services;
using Xamarin.Forms;

namespace UsersList
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        readonly IList<User> users = new ObservableCollection<User>();
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditUserView());
        }
    }
}
