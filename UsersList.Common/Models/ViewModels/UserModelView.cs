using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace UsersList.Common.Models.ViewModels
{
    public class UserModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool isSelected = false;
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}