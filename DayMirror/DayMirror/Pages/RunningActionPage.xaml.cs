using DayMirror.Database;
using DayMirror.Models;
using DayMirror.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunningActionPage : ContentPage
    {
        public double secondsElapsed { get; private set; }

        public RunningActionPage()
        {
            InitializeComponent();
            Device.StartTimer(TimeSpan.FromSeconds(1),TimerTick);
        }

        bool TimerTick()
        {
            Timer.Text = TimeSpan.FromSeconds(++secondsElapsed)
                .ToString(@"hh\:mm\:ss");

            return true;
        }

        async void OnStopActivityButtonClicked(object sender, EventArgs e)
        {
            var action = ((UserActionViewModel)BindingContext);
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