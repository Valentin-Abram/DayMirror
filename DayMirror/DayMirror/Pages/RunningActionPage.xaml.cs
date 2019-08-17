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
            DeviceDisplay.KeepScreenOn = true;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            isPageActive = true;

            TimeSpan timeElapsed = DateTime.Now
                .TimeOfDay.Subtract(GetUserActionModel().StartTime);

            SetTimerValue(timeElapsed);

            double secondsElapsed = timeElapsed.TotalSeconds;

            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                SetTimerValue(TimeSpan.FromSeconds(++secondsElapsed));
                return isPageActive;
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
            var action = GetUserActionModel();
            action.EndTime = DateTime.Now.TimeOfDay;

            var userAction = new UserAction()
            {
                Title = action.Title,
                StartTime = action.StartTime,
                EndTime = action.EndTime,
                UserActionContextId = action.ActionContext?.ID,
                Date = DateTime.Now
            };

            await App.Database.CreateOrUpdateAction(userAction);

            await Navigation.PushAsync(new FinishedActionDetails()
            {
                BindingContext = action,
            });
        }

    }
}