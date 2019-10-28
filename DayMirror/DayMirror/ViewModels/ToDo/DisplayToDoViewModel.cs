using DayMirror.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DayMirror.ViewModels
{
    class DisplayToDoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<UserActionDisplayModel> ToDoList { get; set; } = new ObservableCollection<UserActionDisplayModel>();
        private UserActionDisplayModel _selectedItem;
        public UserActionDisplayModel SelectedItem
        {
            get 
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
                if (_selectedItem != null)
                {
                    RunAction(_selectedItem);
                }
            }
        }


        public DisplayToDoViewModel()
        {
        }

        public async Task LoadData()
        {
            ToDoList.Clear();

            var list = await App.Database.GetToDoListAsync();
            var contextList = await App.Database.GetActionContextsAsync();

            foreach (var item in list)
            {
                ToDoList.Add(
                    new UserActionDisplayModel(
                        item,
                        contextList.FirstOrDefault(c => c.ID == item?.UserActionContextId))
                    );
            }
        }

        public void RunAction(UserActionDisplayModel userActionModel)
        {
            SendMessage("RunAction", userActionModel);
        }

        private void SendMessage(string messageName, object data)
        {
            MessagingCenter.Send<DisplayToDoViewModel, object>(this, messageName, data);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
