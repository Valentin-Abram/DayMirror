using DayMirror.Models.UserAction;
using DayMirror.ViewModels;
using DayMirror.ViewModels.ActionStates;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as RunningActionViewModel).StartTimer();
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            (BindingContext as RunningActionViewModel).StopTimer();
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