using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UsersList.Common.Models.Entities;
using UsersList.Services;
using Xamarin.Forms;

namespace UsersList.Common.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public IUserService _userService { get; set; }
        public ICommand GetUsersCommand => new Command(GetUsers);


        public UserViewModel(UserService userService)
        {
            _userService = userService;
        }

        public User User { get; set; }
        private async void GetUsers()
        {
            var list = await _userService.Get();
        }


        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
