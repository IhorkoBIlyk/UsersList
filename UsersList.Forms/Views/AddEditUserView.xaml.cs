using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersList.Common.Models.Entities;
using UsersList.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UsersList.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditUserView : ContentPage
    {
        User _user;
        public AddEditUserView(User user)
        {
            InitializeComponent();
            _user = user;
        }
        public AddEditUserView()
        {
            InitializeComponent();
        }

        public async void OnSaveButtonClicked(object obj, EventArgs args)
        {
            if(_user == null)
            {
                var user = new User();
                user.Name = name.Text;
                await DependencyService.Get<IUserService>().Create(user);
            }
            else
            {
                _user.Name = name.Text;
                await DependencyService.Get<IUserService>().Edit(_user);
            }

            await Navigation.PopAsync();
        }
    }
}