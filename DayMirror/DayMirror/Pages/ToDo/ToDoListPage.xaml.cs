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
            MessagingCenter.Subscribe<DisplayToDoViewModel, object>(this, "EditAction", EditAction);
            MessagingCenter.Subscribe<DisplayToDoViewModel, object>(this, "DeleteAction", DeleteAction);

        }

        private void RunAction(DisplayToDoViewModel sender, object data)
        {
            var userActionDM = data as UserActionDisplayModel;

            if (userActionDM is null)
            {
                DisplayAlert("Error","Error","Ok");
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

        private async void EditAction(object sender, object data)
        {
            var action = (data as UserActionDisplayModel);

            if(action is null)
            {
                await DisplayAlert("Error", "Cannot navigate to action", "Ok");
                return;
            }

            await GoToEditToDoPage(action.ID);
        }


        private async Task GoToEditToDoPage(int actionId)
        {
            var userAction = await App.Database.GetUserAction(actionId);

            if (userAction is null)
            {
                await DisplayAlert("Error","Cannot navigate to action", "Ok");
                return;
            }

            await Navigation.PushAsync(new EditToDoPage()
            {
                BindingContext = new EditToDoViewModel(userAction)
            });

        }

        private async void DeleteAction(DisplayToDoViewModel sender, object data)
        {
            await DeleteToDoItem((data as UserActionDisplayModel));
        }


        private async Task DeleteToDoItem(UserActionDisplayModel actionDisplayModel)
        { 
            var result = await DisplayAlert("Delete action?",actionDisplayModel.Title, "Ok","Cancel");

            if (result == true)
                await App.Database.DeleteAction(actionDisplayModel.ID);

            await (BindingContext as DisplayToDoViewModel).LoadData();
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