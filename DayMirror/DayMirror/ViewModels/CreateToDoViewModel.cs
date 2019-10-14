using DayMirror.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DayMirror.ViewModels
{
    class CreateToDoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<UserActionContext> ActionContextList { get; set; } = new ObservableCollection<UserActionContext>();
        public ICommand Create { get; set; }

        private string title;
        public string Title 
        {
            get 
            {
                return title; 
            }
            set 
            {
                title = value;
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


        public CreateToDoViewModel()
        {
            LoadData();

            Create = new Command(CreateToDo);
        }



        private async Task LoadData()
        {
            foreach (var item in await App.Database.GetActionContextsAsync())
            {
                ActionContextList.Add(item);
            }
        }

        public void CreateToDo(object )
        {

        }

        #region 
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
