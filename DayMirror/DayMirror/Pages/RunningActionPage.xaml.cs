using DayMirror.Database;
using DayMirror.Models;
using DayMirror.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunningActionPage : ContentPage
    {
        private bool isPageActive { get; set; }
        public RunningActionPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            isPageActive = true;

            UserActionViewModel actionModel = GetUserActionModel();
            actionModel.Status = UserActionStatus.Running;

            TimeSpan timeElapsed = DateTime.Now
                .TimeOfDay.Subtract(actionModel.StartTime);

            SetTimerValue(timeElapsed);

            double secondsElapsed = timeElapsed.TotalSeconds;

            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                SetTimerValue(TimeSpan.FromSeconds(++secondsElapsed));
                return isPageActive;
            });

            Task.Run(async () => 
            {
                var action = await App.Database.CreateOrUpdateAction(actionModel.GetAction());
                await UpdateActionModel(action);
            });

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            isPageActive = false;
        }

        private void SetTimerValue(TimeSpan time)
        {
            Timer.Text = time.ToString(@"hh\:mm\:ss");
        }

        private UserActionViewModel GetUserActionModel()
        {
            return ((UserActionViewModel)BindingContext);
        }

        async void OnStopActivityButtonClicked(object sender, EventArgs e)
        {
            var actionModel = GetUserActionModel();
            actionModel.EndTime = DateTime.Now.TimeOfDay;
            actionModel.Status = UserActionStatus.Finished;

            await App.Database.CreateOrUpdateAction(actionModel.GetAction());

            await Navigation.PushAsync(new FinishedActionDetails()
            {
                BindingContext = actionModel,
            });
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent from go back
            return true;
        }

        private async Task UpdateActionModel(UserAction userAction)
        {
            ((UserActionViewModel)this.BindingContext).Id = userAction.ID;
            ((UserActionViewModel)this.BindingContext).Title = userAction.Title;
            ((UserActionViewModel)this.BindingContext).StartTime = userAction.StartTime;
            ((UserActionViewModel)this.BindingContext).EndTime = userAction.EndTime;
            ((UserActionViewModel)this.BindingContext).Date = userAction.Date;
            ((UserActionViewModel)this.BindingContext).Status = userAction.Status;
            ((UserActionViewModel)this.BindingContext).ActionContext = await App.Database.GetActionContextAsync(userAction.UserActionContextId);
        }
    }
}