using DayMirror.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartActionPage : ContentPage
    {
        public StartActionPage()
        {
            InitializeComponent();
        }

        void OnActionTitleTextChanged(object sender, TextChangedEventArgs e)
        {
            (BindingContext as UserActionViewModel).Title = e.NewTextValue;
        }

        async void OnStartActivityButtonClicked(object sender, EventArgs e)
        {
            var actionVM = (UserActionViewModel)BindingContext;


            if (string.IsNullOrEmpty(actionVM.Title))
            {
                await DisplayAlert("", "Please enter action name", "Cancel");
                return;
            }
            
            actionVM.StartTime = DateTime.Now.TimeOfDay;
            
            var userAction = await App.Database.CreateUserAction(actionVM.GetAction());
            var actionContextTitle = actionVM.ActionContext.Title;

            await Navigation.PushAsync(new Pages.ActionStates.RunningActionPage()
            {
                BindingContext = new ViewModels.ActionStates.RunningActionViewModel(userAction.ID, userAction.Title, actionContextTitle)
            });
        }



        async void OnAddContextButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectActivityContext()
            {
                BindingContext = new UserActionViewModel()
            });
        }

    }
}