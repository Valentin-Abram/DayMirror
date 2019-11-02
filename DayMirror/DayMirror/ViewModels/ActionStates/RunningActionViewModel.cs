using DayMirror.Enums.UserAction;
using DayMirror.Models;
using DayMirror.Models.UserAction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DayMirror.ViewModels.ActionStates
{
    class RunningActionViewModel : INotifyPropertyChanged
    {
        private int Id { get; set; }

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

        private string actionConextTitle;
        public string ActionContextTitle 
        {
            get
            {
                return actionConextTitle;
            }   
            set 
            {
                actionConextTitle = value;
                NotifyPropertyChanged();
            } 
        }

        private TimeSpan timeElapsed;
        public TimeSpan TimeElapsed 
        {
            get
            {
                return timeElapsed;
            }
            set
            {
                timeElapsed = value;
                NotifyPropertyChanged();
            }
        }

        private TimeSpan StartTime = DateTime.Now.TimeOfDay;
        private bool isTimerActive = false;
        
        public ICommand FinishActionCommand { get; private set; }

        public RunningActionViewModel()
        {
            RegisterCommands();
        }

        public RunningActionViewModel(int id) : this()
        {
            LoadData(id);
        }

        public RunningActionViewModel(int id, string title, string actionContextTitle) : this()
        {
            this.Id = id;
            this.title = title; 
            this.actionConextTitle = actionContextTitle;
        }


        private async Task LoadData(int id)
        {
            var userAction = await App.Database.GetUserAction(this.Id);
            var actionContext = await App.Database.GetActionContextAsync(userAction.UserActionContextId);

            this.Id = id;
            this.Title = userAction.Title;
            this.ActionContextTitle = actionContext?.Title;
        }

        private async void FinishAction()
        {
            var userAction = await App.Database.GetUserAction(this.Id);
            userAction.StartTime.Add(timeElapsed);
            userAction.Status = UserActionStatus.Finished;

            userAction = await App.Database.UpdateUserAction(userAction);

            if (userAction != null && userAction.Status == UserActionStatus.Finished)
            {
                MessagingCenter.Send<RunningActionViewModel, UserAction>(this, "FinishedActionMessage", userAction);
            }
            else
            { 
                MessagingCenter.Send<RunningActionViewModel,int>(this, "FailedToFinisActionMessage", this.Id);
            }

        }

        public void StartTimer()
        {
            isTimerActive = true;

            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                TimeElapsed = DateTime.Now.TimeOfDay.Subtract(StartTime);
                return isTimerActive;
            });
        }

        public void StopTimer()
        {
            isTimerActive = false;
        }

        private void RegisterCommands()
        {
            FinishActionCommand = new Command(FinishAction);
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
