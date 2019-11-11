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
        private UserAction _userAction;
        public UserAction UserAction
        {
            get { return _userAction; }
            set 
            { 
                _userAction = value; 
                NotifyPropertyChanged(); 
            }
        }

        private UserActionContext _userActionContext;
        public UserActionContext UserActionContext
        {
            get { return _userActionContext; }
            set { _userActionContext = value;}
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

        private bool isTimerActive = false;
        
        public ICommand FinishActionCommand { get; private set; }
        public ICommand PauseActionCommand { get; private set; }

        public RunningActionViewModel(UserAction userAction, UserActionContext userActionContext = null) 
        {
            this.UserAction = userAction;
            this.UserActionContext = userActionContext;
            RegisterCommands();
            SetupActionForRun();
        }

        private async void FinishAction()
        {
            this.UserAction.EndTime = DateTime.Now.TimeOfDay;
            this.UserAction.Status = UserActionStatus.Finished;

            await App.Database.UpdateUserAction(this.UserAction);

            if (this.UserAction.Status == UserActionStatus.Finished)
            {
                MessagingCenter.Send<RunningActionViewModel, UserAction>(this, "FinishedActionMessage", this.UserAction);
            }
            else
            { 
                MessagingCenter.Send<RunningActionViewModel,int>(this, "FailedToFinisActionMessage", this.UserAction.ID);
            }
        }

        private async void PauseAction()
        {
            this.UserAction.EndTime = DateTime.Now.TimeOfDay;
            this.UserAction.Status = UserActionStatus.Paused;

            await App.Database.UpdateUserAction(this.UserAction);

            if (this.UserAction.Status == UserActionStatus.Paused)
            {
                MessagingCenter.Send<RunningActionViewModel, UserAction>(this, "PausedActionMessage", this.UserAction);
            }
            else
            {
                MessagingCenter.Send<RunningActionViewModel, int>(this, "FailedToPauseActionMessage", this.UserAction.ID);
            }
        }


        private async Task SetupActionForRun()
        {
            if (this.UserAction.Status != UserActionStatus.Running)
            { 
                this.UserAction.StartTime = DateTime.Now.TimeOfDay;
                this.UserAction.Status = UserActionStatus.Running;
                await App.Database.UpdateUserAction(this.UserAction);
            }
        }

        public void StartTimer()
        {
            isTimerActive = true;

            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                TimeElapsed = DateTime.Now.TimeOfDay.Subtract(this.UserAction.StartTime);
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
            PauseActionCommand = new Command(PauseAction);
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
