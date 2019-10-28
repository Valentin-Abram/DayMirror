using DayMirror.Enums.UserAction;
using DayMirror.Models;
using DayMirror.Models.UserAction;
using DayMirror.ViewModels;
using DayMirror.ViewModels.ActionStates;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror.Pages.ActionStates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunningActionPage : ContentPage
    {
        public RunningActionPage()
        {
            InitializeComponent();
            SubscribeToMessages();
        }


        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<RunningActionViewModel, UserAction>(this, "FinishedActionMessage", OnActionFinished);
            MessagingCenter.Subscribe<RunningActionViewModel, int>(this, "FinishedActionMessage", OnFailedToFinisAction);
        }

        private void OnFailedToFinisAction(RunningActionViewModel sender, int actionId)
        {
            DisplayAlert("Error", "Failed to finish action", "ok");
        }

        private async void OnActionFinished(RunningActionViewModel sender, UserAction userAction)
        {
            await Navigation.PushAsync(new FinishedActionDetails()
            {
                BindingContext = await UserActionViewModel.FromAction(userAction)
            });
        }
    }
}