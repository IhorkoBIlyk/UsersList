using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersList.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UsersList.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditUserView : ContentPage
    {
        public AddEditUserView()
        {
            InitializeComponent();
        }

        public async void OnSaveButtonClicked(object obj, EventArgs args)
        {
            var users = await DependencyService.Get<IUserService>().Get();

        }
    }
}