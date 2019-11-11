using DayMirror.Enums.UserAction;
using DayMirror.Models;
using DayMirror.Models.UserAction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DayMirror.ViewModels
{
    public class EditToDoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<UserActionContext> ActionContextList { get; set; } = new ObservableCollection<UserActionContext>();
        public ICommand EditCommand { get; set; }


        private UserAction _userAction;

        public UserAction UserAction
        {
            get
            { 
                return _userAction;
            }
            set 
            {
                _userAction = value;
                NotifyPropertyChanged(); 
            }
        }

        private UserActionContext context;
        public UserActionContext Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;
                NotifyPropertyChanged();
            }
        }


        public EditToDoViewModel(UserAction userAction)
        {
            this.UserAction = userAction;
            LoadData();

            EditCommand = new Command(EditToDo);
        }


        private async Task LoadData()
        {
            foreach (var item in await App.Database.GetActionContextsAsync())
            {
                ActionContextList.Add(item);
            }
            
            Context = ActionContextList
               .Where(c => c.ID == UserAction.UserActionContextId)
               .FirstOrDefault();
        }

        public async void EditToDo()
        {
            if (string.IsNullOrWhiteSpace(this.UserAction.Title))
            {
                SendMessage("Validation", "Title can't be empty");
                return;
            }

            await Save();
        }

        private async Task Save()
        {
            UserAction.UserActionContextId = Context.ID;

            await App.Database.UpdateUserAction(UserAction);

            SendMessage("Edit", "Ok");
        }

        private void SendMessage(string messageName, string message)
        {
            MessagingCenter.Send<EditToDoViewModel, string>(this, messageName, message);
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
