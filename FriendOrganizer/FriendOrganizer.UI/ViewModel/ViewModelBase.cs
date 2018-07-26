﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Prism.Commands;

namespace FriendOrganizer.UI.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaiseCanExecuteChanged(ICommand command)
        {
            ((DelegateCommand)command).RaiseCanExecuteChanged();
        }
    }
}
