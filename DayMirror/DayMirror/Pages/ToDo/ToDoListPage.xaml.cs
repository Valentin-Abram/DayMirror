using DayMirror.Models;
using DayMirror.ViewModels;
using DayMirror.ViewModels.ActionStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror.Pages.ToDo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoListPage : ContentPage
    {
        public ToDoListPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<DisplayToDoViewModel, object>(this, "RunAction", RunAction);

        }

        private void RunAction(DisplayToDoViewModel sender, object data)
        {
            var userActionDM = data as UserActionDisplayModel;

            if (userActionDM is null)
            {
                DisplayAlert("Error lol","Erro===r","Ok");
                return;
            }

            var userActionVM = new UserActionViewModel
            {
                Id = userActionDM.ID,
                Date = userActionDM.Date,
                ActionContext = userActionDM.ActionContext,
                StartTime = userActionDM.StartTime,
                EndTime = userActionDM.EndTime,
                Status = userActionDM.Status,
                Title = userActionDM.Title
            };

            var userAction = userActionVM.GetAction();

            Navigation.PushAsync(new Pages.ActionStates.RunningActionPage()
            {
                BindingContext = new RunningActionViewModel(userAction, userActionDM.ActionContext)
            });
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as DisplayToDoViewModel)?.LoadData();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateToDoPage());
        }
    }
}