using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersList.Common.Models.Entities;
using UsersList.Common.Models.ViewModels;
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
        readonly IList<UserModelView> userModelViews = new ObservableCollection<UserModelView>();
        public MainPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            UpdatePage();
        }

        async void UpdatePage()
        {
            var users = await DependencyService.Get<IUserService>().Get();

            userModelViews.Clear();
            foreach (var user in users)
            {
                UserModelView userModelView = new UserModelView();
                userModelView.Id = user.Id;
                userModelView.Name = user.Name;
                userModelViews.Add(userModelView);
            }

            MainListView.ItemsSource = userModelViews;
        }


        private void MainListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var SelectedItem = e.Item as UserModelView;

            if (SelectedItem != null)
                SelectedItem.IsSelected = !SelectedItem.IsSelected;
        }

        private async void OnCreateUserClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditUserView());
        }
        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            if (MainListView.SelectedItem == null) return;
            var SelectedUser = MainListView.SelectedItem as UserModelView;

            var user = new User();
            user.Id = SelectedUser.Id;
            user.Name = SelectedUser.Name;

            await Navigation.PushAsync(new AddEditUserView(user));

            MainListView.SelectedItem = null;

        }
        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var usersToDelete = userModelViews.Where(u => u.IsSelected == true);

            var users = new List<User>();
            foreach (var user in usersToDelete)
            {
                User userToDelete = new User
                {
                    Id = user.Id,
                    Name = user.Name
                };
                users.Add(userToDelete);
            }

            await DependencyService.Get<IUserService>().DeleteRange(users);
            UpdatePage();
        }
    }
}
